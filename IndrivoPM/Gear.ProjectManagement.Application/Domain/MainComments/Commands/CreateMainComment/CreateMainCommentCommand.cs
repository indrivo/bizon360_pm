using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.MainComments.Commands.CreateMainComment
{
    public class CreateMainCommentCommand : IRequest
    {
        public Guid RecordId { get; set; }

        public string Message { get; set; }
    }
}
