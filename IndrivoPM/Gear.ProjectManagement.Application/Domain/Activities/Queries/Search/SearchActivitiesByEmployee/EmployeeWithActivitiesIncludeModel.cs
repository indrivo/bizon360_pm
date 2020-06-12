using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.AppEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityListFromParentEntity;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchActivitiesByEmployee
{
    public class EmployeeWithActivitiesIncludeModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string JobPosition { get; set; }
        public int CompletedActivitiesCount { get; set; }
        public int ActivitiesCount { get; set; }

        public float EstimatedTime { get; set; }

        public float LoggedTime { get; set; }

        public int Progress { get; set; }

        public ICollection<ActivityLookupModel> Activities { get; set; }

        public static Expression<Func<ApplicationUser, EmployeeWithActivitiesIncludeModel>> SimpleProjection
        {
            get
            {
                return employee => new EmployeeWithActivitiesIncludeModel
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    JobPosition = employee.JobPosition != null ? employee.JobPosition.Name : "-",
                    CompletedActivitiesCount = employee.Activities.Count(a => a.Activity.ActivityStatus == ActivityStatus.Completed),
                    ActivitiesCount = employee.Activities.Count,
                    EstimatedTime = employee.Activities.Any() ? employee.Activities.Sum(a => a.Activity.EstimatedHours ?? 0) : 0,
                    LoggedTime = employee.Activities.Any() ? employee.Activities.Select(a => a.Activity.LoggedTimes.Sum(lt => lt.Time)).Sum() : 0,
                    Progress = employee.Activities.Any() ? (int)employee.Activities.Average(a => a.Activity.Progress) : 0,
                    Activities = employee.Activities.Select(a => ActivityLookupModel.Create(a.Activity)).ToList()
                };
            }
        }
        public static Expression<Func<ApplicationUser, string, int, EmployeeWithActivitiesIncludeModel>> Projection
        {
            get
            {
                return (employee, searchValue, number) => new EmployeeWithActivitiesIncludeModel
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    JobPosition = employee.JobPosition != null ? employee.JobPosition.Name : "-",
                    CompletedActivitiesCount = employee.Activities.Count(a => a.Activity.ActivityStatus == ActivityStatus.Completed),
                    ActivitiesCount = employee.Activities.Count,
                    EstimatedTime = employee.Activities.Any() ? employee.Activities.Sum(a => a.Activity.EstimatedHours ?? 0) : 0,
                    LoggedTime = employee.Activities.Any() ? employee.Activities.Select(a => a.Activity.LoggedTimes.Sum(lt => lt.Time)).Sum() : 0,
                    Progress = employee.Activities.Any() ? (int)employee.Activities.Average(a => a.Activity.Progress) : 0,
                    Activities = employee.Activities.Where(a => a.Activity.Name.ToLower().Contains(searchValue.ToLower()) || a.Activity.Number == number)
                        .Select(a => ActivityLookupModel.Create(a.Activity)).ToList()
                };
            }
        }

        public static EmployeeWithActivitiesIncludeModel Create(ApplicationUser user)
        {
            return SimpleProjection.Compile().Invoke(user);
        }
        public static EmployeeWithActivitiesIncludeModel Create(ApplicationUser user, string searchValue, int number)
        {
            if (string.IsNullOrEmpty(searchValue)) return SimpleProjection.Compile().Invoke(user);

            return Projection.Compile().Invoke(user, searchValue, number);
        }
    }
}
