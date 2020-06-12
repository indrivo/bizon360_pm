using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.MainComments.Queries.GetMainCommentDetails
{
    public class GetMainCommentDetailQuery : IRequest<MainCommentDetailModel>
    {
        public Guid Id { get; set; }
    }
}
