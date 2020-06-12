using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.AppEntities;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityList;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityListByAssignee
{
    public class AssigneeLookupModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string JobPosition { get; set; }

        public ICollection<ActivityLookupModel> Activities { get; set; }

        public int Progress { get; set; }

        public float TotalEstimated { get; set; }

        public float TotalLogged { get; set; }

        public string ProjectName { get; set; }

        public static Expression<Func<ApplicationUser, AssigneeLookupModel>> Projection
        {
            get
            {
                return assignee => new AssigneeLookupModel
                {
                    Id = assignee.Id,
                    FirstName = assignee.FirstName,
                    LastName = assignee.LastName,
                    JobPosition = assignee.JobPosition != null ? assignee.JobPosition.Name : "Not Specified",
                    Activities = assignee.Activities.Select(a => ActivityLookupModel.Create(a.Activity)).ToList()
                };
            }
        }

        public static AssigneeLookupModel Create(ApplicationUser user, Guid projectId, string projectName)
        {
            var projection = Projection.Compile().Invoke(user);

            projection.Activities = projection.Activities.Where(a => a.ProjectId == projectId).ToList();
            projection.TotalEstimated = projection.Activities.Any() ? projection.Activities.Sum(a => a.EstimatedHours ?? 0) : 0;
            projection.TotalLogged = projection.Activities.Any() ? projection.Activities.Sum(a => a.LoggedTime ?? 0) : 0;
            projection.Progress = projection.Activities.Any() ? (int) projection.Activities.Average(a => a.Progress) : 0;
            projection.ProjectName = projectName;

            return projection;
        }
    }
}
