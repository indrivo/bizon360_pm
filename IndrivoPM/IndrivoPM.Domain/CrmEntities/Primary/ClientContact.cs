using System;
using System.Collections.Generic;
using System.Linq;
using Gear.Common.Entities;
using Gear.Common.Extensions.Result;
using Gear.Common.Extensions.ValueObjects;
using Gear.Domain.CrmEntities.ManyToManyEntities;
using Gear.Domain.CrmInterfaces;
using Gear.Domain.ValueObjects;
using Newtonsoft.Json;

namespace Gear.Domain.CrmEntities.Primary
{
    public class ClientContact : BaseModel
    {
        public ContactInfo ContactInfo { get; private set; }

        private string _notes;

        public IEnumerable<ContactNote> Notes => JsonConvert.DeserializeObject<IEnumerable<ContactNote>>(_notes);

        public new CompoundName Name { get; private set; }

        public Guid OrganizationId { get; private set; }

        public ClientOrganization ClientOrganization { get; private set; }

        private HashSet<DealContact> _deals { get; set; }

        public IEnumerable<DealContact> Deals => _deals?.ToList();


        /// <summary>
        /// Private constructor to restrict
        /// the creation of empty contacts
        /// necessary for EF Core.
        /// </summary>
        private ClientContact() { }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="contactInfo"></param>
        /// <param name="organizationId"></param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="createdBy"></param>
        public ClientContact(ContactInfo contactInfo, Guid organizationId,
            CompoundName name, Guid id, Guid createdBy)
        {
            if(organizationId == Guid.Empty)
                throw new ArgumentException("Organization Id must be specified");

            ContactInfo = contactInfo;
            Name = name;

            CreateEnd(id,name.ToString(),createdBy);
        }


        /// <summary>
        /// Update contact info, returns false if Value Objects are equal.
        /// </summary>
        /// <param name="newInfo"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public Result UpdateContactInfo(ContactInfo newInfo, Guid modifiedBy)
        {
            if (ContactInfo.Equals(newInfo))
                return Result.Fail("Contact info is identical");

            ContactInfo = newInfo;

            return Result.Ok();

        }


        /// <summary>
        /// Change the name of the contact
        /// </summary>
        /// <param name="newName"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public Result UpdateContactName(CompoundName newName, Guid modifiedBy)
        {
            if (Name.Equals(newName))
                return Result.Fail("Name is identical");

            Name = newName;

            

            return Result.Ok();
        }


        /// <summary>
        /// Add note to the contact.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="note"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public Result AddNote(ContactNote note, Guid modifiedBy)
        {
            //Fail pushed onto the CQRS pipeline execution
            var existingNotes = JsonConvert.DeserializeObject<ICollection<ContactNote>>(_notes);

            if (!existingNotes.Any(x => x.Equals(note)))
            {
                existingNotes.Add(note);
                _notes = JsonConvert.SerializeObject(existingNotes);
                

                return Result.Ok();
            }

            return Result.Fail($"Note already existing : {note.ToString()}");
        }


        /// <summary>
        /// Remove note from the contact.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="note"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public Result RemoveNote(ICrmContext context, ContactNote note, Guid modifiedBy)
        {
            //Fail pushed onto the CQRS pipeline execution
            var existingNotes = JsonConvert.DeserializeObject<ICollection<ContactNote>>(_notes);

            if (!existingNotes.Any(x => x.Equals(note)))
            {
                existingNotes.Remove(note);
                _notes = JsonConvert.SerializeObject(existingNotes);

                
                context.ClientContacts.Update(this);

                return Result.Ok();
            }

            return Result.Fail($"Trying to remove inexistent note {note.Note}");
        }
        
    }

    /// <summary>
    /// Contact notes.
    /// </summary>
    public class ContactNote : ValueObject
    {
        public string Note { get; private set; }

        public string AuthorName { get; private set; }

        private ContactNote() { }

        protected ContactNote(string note, string authorName)
        {
            this.Note = note;
            this.AuthorName = authorName;
        }

        public Result<ContactNote> Create(string note, string authorName)
        {
            if (string.IsNullOrEmpty(note.Trim()))
                return Result.Fail<ContactNote>("Note was passed empty or null");

            if (string.IsNullOrEmpty(authorName.Trim()))
                return Result.Fail<ContactNote>("Author Name was passed empty or null");

            return Result.Ok(new ContactNote(note, authorName));
        }

        //TODO: Create a awy of reading the data if necessary

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Note;
            yield return AuthorName;
        }
    }

}
