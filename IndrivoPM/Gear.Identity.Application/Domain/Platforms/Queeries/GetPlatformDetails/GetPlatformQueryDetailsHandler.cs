using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Gear.Identity.Permissions.Infrastructure.Attributes;
using Gear.Identity.Permissions.Infrastructure.Resources.ViewModels;
using Gear.Identity.Permissions.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Identity.Manager.Domain.Platforms.Queeries.GetPlatformDetails
{
    public class GetPlatformQueryDetailsHandler : IRequestHandler<GetPlatformQueryDetails, PlatformDetailsModel>
    {
        private readonly IPermissionContext _gearContext;

        public GetPlatformQueryDetailsHandler(IPermissionContext gearContext)
        {
            _gearContext = gearContext;
        }

        public async Task<PlatformDetailsModel> Handle(GetPlatformQueryDetails request, CancellationToken cancellationToken)
        {
            var rawData = await _gearContext.RolesToPermissions.Where(x => x.PlatformName == request.PlatfromName).ToListAsync(cancellationToken);

            var roleDto = new List<RolesListDto>();

            foreach (var roleToPermissions in rawData)
            {
                var permissionsWithDesc =
                    from permission in roleToPermissions.GetPermissionsInRole
                    let displayAttr = typeof(Identity.Permissions.Domain.Resources.Permissions).GetMember(permission.ToString())[0]
                        .GetCustomAttribute<DisplayAttribute>()
                    let moduleAttr = typeof(Identity.Permissions.Domain.Resources.Permissions).GetMember(permission.ToString())[0]
                        .GetCustomAttribute<LinkedToModuleAttribute>()
                    select new PermissionWithDesc(permission.ToString(), displayAttr?.Description, moduleAttr?.PaidForModule.ToString());
                roleDto.Add(new RolesListDto(roleToPermissions.RoleName, roleToPermissions.Description, permissionsWithDesc.ToList(), roleToPermissions.Modified, roleToPermissions.Active));
            }

            return new PlatformDetailsModel()
            {
                PlatformName = request.PlatfromName,
                Roles = roleDto
            };
        }
    }
}
