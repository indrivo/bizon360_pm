using System;
using System.ComponentModel;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.UpdateActivityType
{
    public class UpdateActivityTypeCommand: IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [DisplayName("Abbr.")]
        public string Abbreviation { get; set; }

        [DisplayName("Color badge")]
        public ColorBadge ColorBadge { get; set; }
    }
}
