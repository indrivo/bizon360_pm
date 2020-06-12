using FluentValidation;

namespace Gear.Identity.Manager.Domain.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            RuleFor(x => x.RoleName).MaximumLength(256)
                .NotEmpty();

            RuleFor(x => x.Description).MaximumLength(150)
                .NotEmpty();
        }
    }
}
