using FluentValidation;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.UpdateApplicationUser
{
    public class UpdateApplicationUserCommandValidator:AbstractValidator<UpdateApplicationUserCommand>
    {
        public UpdateApplicationUserCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.FirstName).NotEmpty()
                .MaximumLength(256);

            RuleFor(x => x.LastName).NotEmpty()
                .MaximumLength(256);

            RuleFor(x => x.Email).NotEmpty()
                .EmailAddress();

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(64);
        }
    }
}
