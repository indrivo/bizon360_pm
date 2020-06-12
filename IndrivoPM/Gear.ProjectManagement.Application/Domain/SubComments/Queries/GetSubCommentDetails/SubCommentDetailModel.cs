using System;
using System.Linq.Expressions;
using Gear.Domain;

namespace Gear.ProjectManagement.Manager.Domain.SubComments.Queries.GetSubCommentDetails
{
    public class SubCommentDetailModel
    {
        public Guid Id { get; set; }

        public string Message { get; set; }


        public static Expression<Func<SubComment, SubCommentDetailModel>> Projection
        {
            get
            {
                return subComment => new SubCommentDetailModel
                {
                    Id = subComment.Id,
                    Message = subComment.Message,
                };
            }
        }
        public static SubCommentDetailModel Create(SubComment subComment)
        {
            return Projection.Compile().Invoke(subComment);
        }
    }
}
