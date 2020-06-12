using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.SubComments.Commands.DeleteSubComment
{
    public class DeleteSubCommentCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
