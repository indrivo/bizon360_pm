using System;
using System.Collections.Generic;
using System.ComponentModel;
using Gear.Domain.AppEntities;

namespace Gear.Domain.PmEntities
{
    public class MainComment : BaseComment
    {
        public Guid RecordId { get; private set; }

        public string AuthorJobPosition { get; private set; }

        public ICollection<SubComment> SubComments { get; set; }

        // -------------------------------
        // Constructors

        private MainComment()
        {
        }

        /// <summary>
        /// Create a comment for a entity
        /// </summary>
        /// <param name="recordId">
        /// the entity id for which a comment was added
        /// </param>
        /// <param name="author">
        /// Pass the user of type ApplicationUser
        /// </param>
        /// <param name="message"></param>
        public MainComment(Guid recordId, ApplicationUser author, string message)
        {
            if (recordId == Guid.Empty) throw new InvalidEnumArgumentException("RecordId should not be empty");

            RecordId = recordId;

            if (author == null) throw new NullReferenceException("The user should not be null");

            AuthorJobPosition = author.JobPosition?.Name ?? "None";

            CreateEnd(author, message);
        }
    }
}
