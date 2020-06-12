using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.MainComments.Commands.UpdateMainComment
{
    public class UpdateMainCommentCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Message { get; set; }
    }
}
