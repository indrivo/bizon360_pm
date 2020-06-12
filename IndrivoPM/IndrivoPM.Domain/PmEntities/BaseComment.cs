using System;
using Gear.Domain.AppEntities;

namespace Gear.Domain.PmEntities
{
    public abstract class BaseComment
    {
        public Guid Id { get; private set; }

        public Guid AuthorId { get; private set; }

        public string AuthorName { get; private set; }
        
        public string Message { get; private set; }

        public DateTime CreatedTime { get; private set; }


        /// <summary>
        /// Create a base comment
        /// </summary>
        /// <param name="author">
        /// Pass the user
        /// </param>
        /// <param name="message"></param>
        protected void CreateEnd(ApplicationUser author, string message)
        {
            Id = Guid.NewGuid();

            if (string.IsNullOrEmpty(author.FirstName)) throw new InvalidOperationException("Name should not be empty");
            AuthorName = author.FirstName + " " + author.LastName;

            if (author.Id == Guid.Empty) throw new InvalidOperationException("Id should not be empty");
            AuthorId = author.Id;

            if (string.IsNullOrEmpty(message)) throw new InvalidOperationException("Message should not be empty");
            Message = message;

            CreatedTime = DateTime.Now;
        }

        /// <summary>
        /// Update comment
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="message"></param>
        public void UpdateComment(BaseComment comment, string message)
        {
            if (string.IsNullOrEmpty(message)) throw new InvalidOperationException("Message should not be empty");

            comment.Message = message;
           // comment.CreatedTime = DateTime.Now;
        }
    }
}
