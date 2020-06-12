using FluentValidation;

namespace Gear.Identity.Manager.Domain.Roles.Commands.DeleteRole
{
    public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidator()
        {
            RuleFor(x => x.RoleNames).NotNull();
        }
    }
}
