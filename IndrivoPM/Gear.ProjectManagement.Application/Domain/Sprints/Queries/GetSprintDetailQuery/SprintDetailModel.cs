using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Queries.GetSprintDetailQuery
{
    public class SprintDetailModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public SprintStatus Status { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public static Expression<Func<Sprint, SprintDetailModel>> Projection
        {
            get
            {
                return sprint => new SprintDetailModel
                {
                    Id = sprint.Id,
                    Name = sprint.Name,
                    Status = sprint.SprintStatus,
                    StartDate = sprint.StartDate,
                    EndDate = sprint.EndDate
                };
            }
        }

        public static SprintDetailModel Create(Sprint sprint)
        {
            return Projection.Compile().Invoke(sprint);
        }
    }
}
