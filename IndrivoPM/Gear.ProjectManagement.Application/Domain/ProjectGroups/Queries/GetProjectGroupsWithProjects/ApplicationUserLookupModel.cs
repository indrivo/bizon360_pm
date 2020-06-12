using System;
using System.Linq.Expressions;
using Gear.Domain.AppEntities;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupsWithProjects
{
    public class ApplicationUserLookupModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string JobPosition { get; set; }

        public static Expression<Func<ApplicationUser, ApplicationUserLookupModel>> Projection
        {
            get
            {
                return applicationUser => new ApplicationUserLookupModel
                {
                    Id = applicationUser.Id,
                    FirstName = applicationUser.FirstName,
                    LastName = applicationUser.LastName,
                    JobPosition = applicationUser.JobPosition != null ? applicationUser.JobPosition.Name : "-"
                };
            }
        }

        public static ApplicationUserLookupModel Create(ApplicationUser applicationUser)
        {
            return Projection.Compile().Invoke(applicationUser);
        }
    }
}