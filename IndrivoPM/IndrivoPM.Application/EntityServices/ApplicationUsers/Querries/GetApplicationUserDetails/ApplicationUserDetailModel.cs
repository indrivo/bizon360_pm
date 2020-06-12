using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.AppEntities;
using Gear.Domain.HrmEntities.Enums;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserDetails
{
    public class ApplicationUserDetailModel
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => FirstName + " " + LastName;
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string JobPositionName { get; set; }

        public Guid? JobPositionId { get; set; }

        public bool Active { get; set; }

        public EmploymentType EmploymentType { get; set; }

        public Dictionary<Guid, string> BusinessUnits { get; set; }

        public Dictionary<Guid, string> Departments { get; set; }

        public List<Guid> TeamIds { get; set; }

        public Dictionary<Guid, string> Teams { get; set; }

        public IList<string> RoleNames { get; set; }
        public List<Guid> Projects { get; set; }
        public DateTime Created { get; set; }
        public static Expression<Func<ApplicationUser, ApplicationUserDetailModel>> Projection
        {
            get
            {
                return applicationUser => new ApplicationUserDetailModel
                {
                    Id = applicationUser.Id,
                    FirstName = !string.IsNullOrEmpty(applicationUser.FirstName) ? applicationUser.FirstName : "None",
                    LastName = !string.IsNullOrEmpty(applicationUser.LastName) ? applicationUser.LastName : "None",
                    Email = applicationUser.Email,
                    PhoneNumber = applicationUser.PhoneNumber,
                    Username = applicationUser.UserName,
                    Active = applicationUser.Active,
                    JobPositionName = applicationUser.JobPosition != null ? applicationUser.JobPosition.Name : null,
                    JobPositionId = applicationUser.JobPositionId,
                    EmploymentType = applicationUser.EmploymentType,
                    Teams = applicationUser.UserDepartmentTeams.Select(x => x.DepartmentTeam).ToDictionary(x => x.Id, x => x.Name),
                    TeamIds = applicationUser.UserDepartmentTeams.Select(x => x.DeparmentTeamId).ToList(),
                    Projects = applicationUser.Projects.Select(x => x.ProjectId).ToList(),
                    BusinessUnits = applicationUser.BusinessUnits != null ? applicationUser.BusinessUnits.ToDictionary(x => x.Id, x => x.Name):null,
                    Departments = applicationUser.Department != null ? applicationUser.Department.ToDictionary(x => x.Id, x => x.Name) : null,
                    Created = applicationUser.CreatedTime
                };
            }
        }

        public static ApplicationUserDetailModel Create(ApplicationUser applicationUser)
        {
            return Projection.Compile().Invoke(applicationUser);
        }
    }
}
