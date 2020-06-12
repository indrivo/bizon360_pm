using System;
using Gear.Common.Entities;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.CreateCheckItem
{
    public class CreateCheckItemCommand : BaseModel, IRequest
    {
        public new Guid Id { get; set; } = Guid.NewGuid();

        public string Content { get; set; } = "No content Provided";

        public Guid ActivityId { get; set; }

    }
}
