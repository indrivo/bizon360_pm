using System;
using Gear.Domain.AppEntities;
using Gear.Domain.PmEntities;

namespace Gear.Domain
{
    public class SubComment : BaseComment
    {
        public string AuthorJobPosition { get; private set; }

        public Guid MainCommentId { get; private set; }

        public MainComment MainComment { get; set; }

        // ----------------------------
        // Constructors

        private SubComment()
        {
        }

        /// <summary>
        /// Add sub comment/replay
        /// </summary>
        /// <param name="mainCommentId">
        /// Reference to main comment
        /// </param>
        /// <param name="author"></param>
        /// <param name="message"></param>
        public SubComment(Guid mainCommentId, ApplicationUser author, string message)
        {
            if (mainCommentId == Guid.Empty) throw new InvalidOperationException("mainCommentId should not be empty");

            MainCommentId = mainCommentId;

            if (author == null) throw new NullReferenceException("The user should not be null");

            AuthorJobPosition = author.JobPosition?.Name ?? "None";

            CreateEnd(author, message);
        }
    }
}
