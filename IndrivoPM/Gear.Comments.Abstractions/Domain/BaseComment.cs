using System;

namespace Gear.Comments.Abstractions.Domain
{
    public abstract class BaseComment
    {
        public Guid Id { get; set; }

        public Guid AuthorId { get; set; }

        public string AuthorName { get; set; }
        
        public string Message { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
