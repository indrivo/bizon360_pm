using System;
using System.Linq.Expressions;
using Gear.Domain.AppEntities;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserLite
{
    public class ApplicationUserLiteModel
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => FirstName + " " + LastName;

        public string PhoneNumber { get; set; }

        public bool Active { get; set; }

        public static Expression<Func<ApplicationUser, ApplicationUserLiteModel>> Projection
        {

            get
            {
                return applicationUser => new ApplicationUserLiteModel
                {
                    Id = applicationUser.Id,
                    FirstName = !string.IsNullOrEmpty(applicationUser.FirstName) ? applicationUser.FirstName : "None",
                    LastName = !string.IsNullOrEmpty(applicationUser.LastName) ? applicationUser.LastName : "None",
                    PhoneNumber = applicationUser.PhoneNumber,
                    Username = applicationUser.UserName,
                    Active = applicationUser.Active,
                };
            }
        }

        public static ApplicationUserLiteModel Create(ApplicationUser applicationUser)
        {
            return Projection.Compile().Invoke(applicationUser);
        }
    }
}
