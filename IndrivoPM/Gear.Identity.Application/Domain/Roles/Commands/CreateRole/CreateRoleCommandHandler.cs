using System;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.AppEntities;
using Gear.Identity.Permissions.Domain.Entities;
using Gear.Identity.Permissions.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Gear.Identity.Manager.Domain.Roles.Commands.CreateRole
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand>
    {
        private readonly IPermissionContext _context;

        private readonly RoleManager<ApplicationRole> _roleManager;

        public CreateRoleCommandHandler(IPermissionContext context, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<Unit> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {

            await _roleManager.CreateAsync(new ApplicationRole()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                NormalizedName = request.Name.Normalize(),
                //TODO: send the platform name here
                PlatformName = "PM"
            });

            await _context.RolesToPermissions.AddAsync
                (new RoleToPermissions(request.Name,
                request.Description, 
                request.Permissions, 
                request.PlatformName, 
                DateTime.Now),cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
