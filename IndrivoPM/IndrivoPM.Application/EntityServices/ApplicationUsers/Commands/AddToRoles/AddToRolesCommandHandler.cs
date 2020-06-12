using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.AppEntities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.AddToRoles
{
    public class AddToRolesCommandHandler : IRequestHandler<AddToRolesCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AddToRolesCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Unit> Handle(AddToRolesCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null) throw new NotFoundException(nameof(ApplicationUser), request.UserId);

            var currentUserRoles = await _userManager.GetRolesAsync(user);

            if (request.RoleNames == null)
            {
                await _userManager.RemoveFromRolesAsync(user, currentUserRoles);
                return Unit.Value;
            }

            if (currentUserRoles.Count == 0)
            {
                await _userManager.AddToRolesAsync(user, request.RoleNames);
                return Unit.Value;
            }

            var addUserRoles = request.RoleNames.Except(currentUserRoles).ToList();

            var removeUserRoles = currentUserRoles.Except(request.RoleNames).ToList();

            if (addUserRoles.Count() != 0) await _userManager.AddToRolesAsync(user, addUserRoles);

            if (removeUserRoles.Count() != 0) await _userManager.RemoveFromRolesAsync(user, removeUserRoles);

            return Unit.Value;
        }
    }
}
