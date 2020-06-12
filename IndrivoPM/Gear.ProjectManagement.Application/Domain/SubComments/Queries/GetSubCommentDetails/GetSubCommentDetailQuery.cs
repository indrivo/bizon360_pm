using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.SubComments.Queries.GetSubCommentDetails
{
    public class GetSubCommentDetailQuery : IRequest<SubCommentDetailModel>
    {
        public Guid Id { get; set; }
    }
}
