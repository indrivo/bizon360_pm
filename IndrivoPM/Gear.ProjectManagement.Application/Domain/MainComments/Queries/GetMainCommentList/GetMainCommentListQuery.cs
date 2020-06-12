using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.MainComments.Queries.GetMainCommentList
{
    public class GetMainCommentListQuery : IRequest<MainCommentListViewModel>
    {
        public Guid RecordId { get; set; }
    }
}
