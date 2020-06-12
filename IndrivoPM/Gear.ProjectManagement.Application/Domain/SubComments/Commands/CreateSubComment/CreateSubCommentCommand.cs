using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.SubComments.Commands.CreateSubComment
{
    public class CreateSubCommentCommand : IRequest
    {
        public Guid MainCommentId { get; set; }

        public string Message { get; set; }
    }
}
