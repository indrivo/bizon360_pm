using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.SubComments.Queries.GetSubCommentList
{
    public class GetSubCommentListQuery : IRequest<SubCommentListViewModel>
    {
        public Guid MainCommentId { get; set; }
    }
}
