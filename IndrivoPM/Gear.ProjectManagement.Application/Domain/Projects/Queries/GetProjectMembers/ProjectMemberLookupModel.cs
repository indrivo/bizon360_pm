using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.AppEntities;
using Gear.Domain.PmEntities.Enums;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectMembers
{
    public class ProjectMemberLookupModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string JobPosition { get; set; }

        public int CompletedActivitiesCount { get; set; }

        public int ActivitiesCount { get; set; }
        public int TotalActivitiesCount { get; set; }

        public float EstimatedTime { get; set; }

        public float LoggedTime { get; set; }

        public static Expression<Func<ApplicationUser, ProjectMemberLookupModel>> SimpleProjection
        {
            get
            {
                return applicationUser => new ProjectMemberLookupModel
                {
                    Id = applicationUser.Id,
                    FirstName = applicationUser.FirstName,
                    LastName = applicationUser.LastName,
                    JobPosition = applicationUser.JobPosition != null ? applicationUser.JobPosition.Name : "-",
                    CompletedActivitiesCount = applicationUser.Activities.Count(a => a.Activity.ActivityStatus == ActivityStatus.Completed),
                    ActivitiesCount = applicationUser.Activities.Count,
                    TotalActivitiesCount = applicationUser.Activities.Count,
                    EstimatedTime = applicationUser.Activities.Any() ? applicationUser.Activities.Sum(a => a.Activity.EstimatedHours ?? 0) : 0,
                    LoggedTime = applicationUser.Activities.Any() ? applicationUser.Activities.Select(a => a.Activity.LoggedTimes.Sum(lt => lt.Time)).Sum() : 0
                };
            }
        }

        public static Expression<Func<ApplicationUser, ICollection<ActivityStatus>, ProjectMemberLookupModel>> Projection
        {
            get
            {
                return (applicationUser, filter) => new ProjectMemberLookupModel
                {
                    Id = applicationUser.Id,
                    FirstName = applicationUser.FirstName,
                    LastName = applicationUser.LastName,
                    JobPosition = applicationUser.JobPosition != null ? applicationUser.JobPosition.Name : "-",
                    CompletedActivitiesCount = applicationUser.Activities.Count(a => a.Activity.ActivityStatus == ActivityStatus.Completed),
                    ActivitiesCount = applicationUser.Activities.Count(a => filter.Any(tag => a.Activity.ActivityStatus == tag)),
                    TotalActivitiesCount = applicationUser.Activities.Count,
                    EstimatedTime = applicationUser.Activities.Any() ? applicationUser.Activities.Sum(a => a.Activity.EstimatedHours ?? 0) : 0,
                    LoggedTime = applicationUser.Activities.Any() ? applicationUser.Activities.Select(a => a.Activity.LoggedTimes.Sum(lt => lt.Time)).Sum() : 0
                };
            }
        }

        public static ProjectMemberLookupModel Create(ApplicationUser user)
        {
            return SimpleProjection.Compile().Invoke(user);
        }
        public static ProjectMemberLookupModel Create(ApplicationUser user, ICollection<ActivityStatus> filter)
        {
            if (filter == null || !filter.Any()) return SimpleProjection.Compile().Invoke(user);

            return Projection.Compile().Invoke(user, filter);
        }
    }
}
