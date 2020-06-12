using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.ProjectManagement.Manager.Domain.SubComments.Queries.GetSubCommentList;

namespace Gear.ProjectManagement.Manager.Domain.MainComments.Queries.GetMainCommentList
{
    public class MainCommentLookupModel 
    {
        public Guid Id { get; set; }

        public Guid RecordId { get; set; }

        public Guid AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string AuthorJobPosition { get; set; }

        public string Message { get; set; }

        public DateTime CreatedTime { get; set; }

        public IList<SubCommentLookupModel> SubComments { get; set; }

        public static Expression<Func<MainComment, MainCommentLookupModel>> Projection
        {
            get
            {
                return comment => new MainCommentLookupModel
                {
                    Id = comment.Id,
                    RecordId = comment.RecordId,
                    AuthorId = comment.AuthorId,
                    AuthorName = comment.AuthorName,
                    AuthorJobPosition = comment.AuthorJobPosition,
                    Message = comment.Message,
                    CreatedTime = comment.CreatedTime,
                    SubComments = comment.SubComments.Select(x => SubCommentLookupModel.Create(x)).ToList()
                };
            }
        }

        public static MainCommentLookupModel Create(MainComment comment)
        {
            return Projection.Compile().Invoke(comment);
        }
    }
}
