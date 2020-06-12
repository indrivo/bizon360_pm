using System;
using System.ComponentModel;
using MediatR;

namespace Gear.Manager.Core.EntityServices.JobPositions.Commands.UpdateJobPosition
{
    public class UpdateJobPositionCommand: IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [DisplayName("Hourly Salary")]
        public int? HourlySalary { get; set; }

        public string Description { get; set; }

        [DisplayName("Status")]
        public bool Active { get; set; }

        [DisplayName("Abbr.")]
        public string Abbreviation { get; set; }

        [DisplayName("Team")]
        public Guid? DepartmentTeamId { get; set; }
    }
}
