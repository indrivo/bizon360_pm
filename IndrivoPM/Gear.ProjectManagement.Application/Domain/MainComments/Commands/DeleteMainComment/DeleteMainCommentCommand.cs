using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.MainComments.Commands.DeleteMainComment
{
    public class DeleteMainCommentCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
