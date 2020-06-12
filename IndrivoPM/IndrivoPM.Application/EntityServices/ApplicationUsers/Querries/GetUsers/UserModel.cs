using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.AppEntities;
using Gear.Domain.HrmEntities.Enums;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetUsers
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public EmploymentType EmploymentType { get; set; }
        public ICollection<ProjectDto> Projects { get; set; }

        public static Expression<Func<ApplicationUser, UserModel>> Projection
        {
            get
            {
                return applicationUser => new UserModel
                {
                    Id = applicationUser.Id,
                    FirstName = string.IsNullOrEmpty(applicationUser.FirstName) ? string.Empty : applicationUser.FirstName,
                    LastName = string.IsNullOrEmpty(applicationUser.LastName) ? string.Empty : applicationUser.LastName,
                    EmploymentType = applicationUser.EmploymentType,
                    Projects = applicationUser.Projects.Select(x => ProjectDto.Create(x.Project, applicationUser.Id)).ToList(),
                };
            }
        }

        public static UserModel Create(ApplicationUser applicationUser) => Projection.Compile().Invoke(applicationUser);
    }
}
