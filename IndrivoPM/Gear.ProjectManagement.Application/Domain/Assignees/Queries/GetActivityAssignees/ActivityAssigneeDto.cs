using System;
using System.Linq.Expressions;
using Gear.Domain.AppEntities;
using Gear.Domain.PmEntities;

namespace Gear.ProjectManagement.Manager.Domain.Assignees.Queries.GetActivityAssignees
{
    public class ActivityAssigneeDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public static Expression<Func<ApplicationUser, ActivityAssigneeDto>> Projection
        {
            get
            {
                return assignee => new ActivityAssigneeDto
                {
                    Id = assignee.Id,
                    FirstName = assignee.FirstName,
                    LastName = assignee.LastName
                };
            }
        }

        public static ActivityAssigneeDto Create(ApplicationUser assignee)
        {
            return Projection.Compile().Invoke(assignee);
        }
    }
}
