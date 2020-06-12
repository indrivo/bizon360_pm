using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.AppEntities;
using Gear.Domain.HrmEntities.Enums;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserList
{
    /// <summary>
    /// Application User EndPoint
    /// </summary>
    public class ApplicationUserLookupModel : IEquatable<ApplicationUserLookupModel>
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
       
        public string FullName => $"{FirstName} {LastName}";

        public string PhoneNumber { get; set; }

        public string JobPosition { get; set; }

        public bool Active { get; set; }

        public List<string> Roles { get; set; } = new List<string>();

        public EmploymentType EmploymentType { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime ModifiedTime { get; set; }

        public static Expression<Func<ApplicationUser, ApplicationUserLookupModel>> Projection
        {
            get
            {
                return applicationUser => new ApplicationUserLookupModel
                {
                    Id = applicationUser.Id,
                    Username = applicationUser.UserName,
                    FirstName = applicationUser.FirstName,
                    LastName = applicationUser.LastName,
                    PhoneNumber = applicationUser.PhoneNumber,
                    EmploymentType = applicationUser.EmploymentType,
                    Active = applicationUser.Active,
                    CreatedTime = applicationUser.CreatedTime,
                    ModifiedTime = applicationUser.ModifiedTime,
                    JobPosition = applicationUser.JobPosition != null ? applicationUser.JobPosition.Name : "None"
                };
            }
        }

        public static ApplicationUserLookupModel Create(ApplicationUser applicationUser)
        {
            return Projection.Compile().Invoke(applicationUser);
        }

        public bool Equals(ApplicationUserLookupModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ApplicationUserLookupModel) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
