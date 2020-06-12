using FluentValidation;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.AddToRoles
{
    public class AddToRolesCommandValidator : AbstractValidator<AddToRolesCommand>
    {
        public AddToRolesCommandValidator()
        {
            RuleFor(x => x.UserId).NotNull().NotEmpty();
        }
    }
}
