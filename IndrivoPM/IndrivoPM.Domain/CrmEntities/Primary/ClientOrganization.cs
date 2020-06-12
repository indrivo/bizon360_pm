using System;
using System.Collections.Generic;
using System.Linq;
using Gear.Common.Entities;
using Gear.Common.Extensions.Result;
using Gear.Domain.ValueObjects;

namespace Gear.Domain.CrmEntities.Primary
{
    public class ClientOrganization : BaseModel
    {
        public Guid? OwnerId { get; private set; }

        public ClientContact Owner { get; private set; }

        public Address Address { get; private set; }

        public ContactInfo ContactInfo { get; private set; }


        private HashSet<ClientContact> _contacts { get; set; }

        public IEnumerable<ClientContact> Contacts => _contacts?.ToList();



        private HashSet<Deal> _deals { get; set; }

        public IEnumerable<Deal> Deals => _deals?.ToList();
 


        /// <summary>
        /// Private constructor to restrict
        /// the creation of empty organizations
        /// necessary for EF Core.
        /// </summary>
        [Obsolete("Only for model binders and EF, don't use it in your code", true)]
        private ClientOrganization()
        {
            //
        }


        /// <summary>
        /// Client organization constructor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="address"></param>
        /// <param name="contactInfo"></param>
        /// <param name="createdBy"></param>
        public ClientOrganization(Guid id, string name, Address address, ContactInfo contactInfo, Guid createdBy)
        {
            if (address != null)
                Address = address;

            if (contactInfo != null)
                ContactInfo = contactInfo;

            CreateEnd(id, name, createdBy);
        }


        /// <summary>
        /// Add contact to the organization.
        /// <remarks>Save changes after the method call.</remarks>
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public Result AddContact(ClientContact contact, Guid modifiedBy)
        {
            //Check if organization id match and if organization is active
            if (contact.OrganizationId != Id)
                return Result.Fail("Organization Id's do not match");

            //Check if this organization contains the contact if not add
            if (_contacts != null && _contacts.All(x => x.Name != contact.Name))
            {
                //Add entity to the aggregate
                _contacts.Add(contact);
            }
            else
            {
                return Result.Fail("Please load the client contacts before working with the entity");
            }


            return Result.Ok();
        }


        /// <summary>
        /// Remove contact from the contact list
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public Result RemoveContact(ClientContact contact, Guid modifiedBy)
        {
            //Check if organization id match and if organization is active
            if (contact.OrganizationId != Id)
                return Result.Fail("Id's do not match");

            //Populate the field if it is null
            if (_contacts == null)
            {
                return Result.Fail("Please load the client contacts before working with the entity");
            }

            //Remove entity from the aggregate
            _contacts.Remove(contact);

            

            return Result.Ok();
        }


        /// <summary>
        /// Set a specific user as the owner of the company.
        /// <remarks>Save changes after the method call.</remarks>
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public Result SetOwner(ClientContact owner, Guid modifiedBy)
        {
            //Check if the ownerId is empty
            if (Owner.Id == Guid.Empty )
                return Result.Fail("New owner id is empty");

            //Check if the owner is part of the organization
            if (Owner.OrganizationId != Id)
                return Result.Fail("Owner must be part of the organization");

            //Check if the owner Id is not identical and is not null
            if (OwnerId != null && OwnerId != owner.Id)
            {
                Owner = Owner;
                OwnerId = owner.Id;
                
                return Result.Ok();
            }
        
            return Result.Fail("Ower Id's do match");
            
        }
    }
}
