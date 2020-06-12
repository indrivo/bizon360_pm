using System;
using System.Linq.Expressions;
using Gear.Domain;

namespace Gear.ProjectManagement.Manager.Domain.SubComments.Queries.GetSubCommentList
{
    public class SubCommentLookupModel
    {
        public Guid Id { get; set; }

        public Guid MainCommentId { get; set; }

        public Guid AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string AuthorJobPosition { get; set; }

        public string Message { get; set; }

        public DateTime CreatedTime { get; set; }

        public static Expression<Func<SubComment, SubCommentLookupModel>> Projection
        {
            get
            {
                return subComment => new SubCommentLookupModel
                {
                    Id = subComment.Id,
                    MainCommentId = subComment.MainCommentId,
                    AuthorId = subComment.AuthorId,
                    AuthorName = subComment.AuthorName,
                    AuthorJobPosition = subComment.AuthorJobPosition,
                    Message = subComment.Message,
                    CreatedTime = subComment.CreatedTime
                };
            }
        }

        public static SubCommentLookupModel Create(SubComment subComment)
        {
            return Projection.Compile().Invoke(subComment);
        }
    }
}
