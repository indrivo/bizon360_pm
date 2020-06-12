using System;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.HrmEntities;

namespace Gear.Manager.Core.EntityServices.JobPositions.Queries.GetJobPositionDetails
{
    public class JobPositionDetailModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? HourlySalary { get; set; }

        public bool Active { get; set; }

        public string Abbreviation { get; set; }

        public Guid? DepartmentTeamId { get; set; }

        public static Expression<Func<JobPosition, JobPositionDetailModel>> Projection
        {
            get
            {
                return jobPosition => new JobPositionDetailModel
                {
                    Id = jobPosition.Id,
                    Name = jobPosition.Name,
                    Active = jobPosition.Active,
                    Description = jobPosition.Description,
                    HourlySalary = jobPosition.HourlySalary,
                    Abbreviation = jobPosition.Abbreviation,
                    DepartmentTeamId = jobPosition.JobDepartmentTeams.Count == 0 ? Guid.Empty : jobPosition.JobDepartmentTeams.Select(x => x.DepartmentTeamId).First()
                };
            }
        }
        public static JobPositionDetailModel Create(JobPosition jobPosition)
        {
            return Projection.Compile().Invoke(jobPosition);
        }
    }
}
