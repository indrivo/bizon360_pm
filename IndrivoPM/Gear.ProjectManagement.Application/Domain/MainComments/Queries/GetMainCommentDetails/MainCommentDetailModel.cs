using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;

namespace Gear.ProjectManagement.Manager.Domain.MainComments.Queries.GetMainCommentDetails
{
    public class MainCommentDetailModel
    {
        public Guid Id { get; set; }

        public string Message { get; set; }


        public static Expression<Func<MainComment, MainCommentDetailModel>> Projection
        {
            get
            {
                return comment => new MainCommentDetailModel
                {
                    Id = comment.Id,
                    Message = comment.Message,
                };
            }
        }
        public static MainCommentDetailModel Create(MainComment comment)
        {
            return Projection.Compile().Invoke(comment);
        }
    }
}
