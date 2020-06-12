using System;
using System.Linq.Expressions;
using Gear.Domain.HrmEntities;

namespace Gear.Manager.Core.EntityServices.JobPositions.Queries.GetJobPositionList
{
    public class JobPositionLookupModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int? HourlySalary { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public string Abbreviation { get; set; }

        public int RowOrder { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime ModifiedTime { get; set; }

        public static Expression<Func<JobPosition, JobPositionLookupModel>> Projection
        {
            get
            {
                return jobPosition => new JobPositionLookupModel
                {
                    Id = jobPosition.Id,
                    Name = jobPosition.Name,
                    Description = jobPosition.Description,
                    HourlySalary = jobPosition.HourlySalary,
                    Active = jobPosition.Active,
                    Abbreviation = jobPosition.Abbreviation,
                    RowOrder = jobPosition.RowOrder,
                    CreatedTime = jobPosition.CreatedTime,
                    ModifiedTime = jobPosition.ModifyTime
                };
            }
        }

        public static JobPositionLookupModel Create(JobPosition jobPosition)
        {
            return Projection.Compile().Invoke(jobPosition);
        }
    }
}
