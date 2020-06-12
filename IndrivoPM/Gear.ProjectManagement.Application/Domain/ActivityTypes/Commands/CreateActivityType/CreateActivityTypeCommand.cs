using System.ComponentModel;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.CreateActivityType
{
    public class CreateActivityTypeCommand : IRequest
    {
        public string Name { get; set; }

        [DisplayName("Abbr.")]
        public string Abbreviation { get; set; }

        [DisplayName("Color badge")]
        public ColorBadge ColorBadge { get; set; }
    }
}
