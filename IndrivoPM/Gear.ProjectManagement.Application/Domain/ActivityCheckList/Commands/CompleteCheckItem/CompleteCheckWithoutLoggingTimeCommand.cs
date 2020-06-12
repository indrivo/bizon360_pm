using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.CompleteCheckItem
{
    public class CompleteCheckWithoutLoggingTimeCommand : IRequest
    {
        public Guid Id { get; set; }

        public Guid ActivityId { get; set; }
    }
}
