using System;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.JobPositions.Commands.CreateJobPosition
{
    public class CreateJobPositionCommand: IRequest
    {
        public string Name { get; set; }

        [DisplayName("Hourly Salary")]
        public int? HourlySalary { get; set; }

        public string Description { get; set; }

        [DisplayName("Team")]
        public Guid? DepartmentTeamId { get; set; }

        [DisplayName("Status")]
        public bool Active { get; set; } = true;

        [DisplayName("Abbr.")]
        public string Abbreviation { get; set; }

    }
}
