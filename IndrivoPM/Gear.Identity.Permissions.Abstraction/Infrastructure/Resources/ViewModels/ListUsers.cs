using System.Collections.Generic;
using System.Linq;
using Gear.Identity.Permissions.Domain.Resources;
using Gear.Identity.Permissions.Services;

namespace Gear.Identity.Permissions.Infrastructure.Resources.ViewModels
{
    public class ListUsers
    {
        private readonly IPermissionContext _applicationDbContext;

        public ListUsers(IPermissionContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public List<UserListDto> ListUserWithRolesAndModules()
        {
            var rolesWithUserIds = _applicationDbContext.UserRoles
                .Select(x => new { _applicationDbContext.Roles.Single(y => y.Id == x.RoleId).Name, x.UserId }).ToList();

            var result = new List<UserListDto>();
            foreach (var user in _applicationDbContext.ApplicationUsers)
            {
                var thisUserModules = _applicationDbContext.ModulesForUsers.Find(user.Id)?.AllowedPaidForModules ??
                                      PaidForModules.None;
                result.Add(new UserListDto(user.UserName,
                    string.Join(", ", rolesWithUserIds.Where(x => x.UserId == user.Id).Select(x => x.Name)),
                    thisUserModules.ToString()
                ));
            }

            return result;
        }
    }
}
