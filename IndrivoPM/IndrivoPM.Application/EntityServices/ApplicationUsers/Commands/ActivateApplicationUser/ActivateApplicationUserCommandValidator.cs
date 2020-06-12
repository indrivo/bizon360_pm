using FluentValidation;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.ActivateApplicationUser
{
    public class ActivateApplicationUserCommandValidator:AbstractValidator<ActivateApplicationUserCommand>
    {
        public ActivateApplicationUserCommandValidator()
        {
            RuleFor(x => x.Ids).NotNull().NotEmpty();

            RuleFor(x => x.Active).NotNull();
        }
    }
}
