using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.SubComments.Commands.UpdateSubComment
{
    public class UpdateSubCommentCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Message { get; set; }
    }
}
