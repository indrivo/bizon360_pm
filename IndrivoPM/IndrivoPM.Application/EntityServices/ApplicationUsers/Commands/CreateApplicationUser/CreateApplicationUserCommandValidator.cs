using FluentValidation;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.CreateApplicationUser
{
    public class CreateApplicationUserCommandValidator:AbstractValidator<CreateApplicationUserCommand>
    {
        public CreateApplicationUserCommandValidator()
        {
            //RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.FirstName).NotEmpty()
                .MaximumLength(256);

            RuleFor(x => x.LastName).NotEmpty()
                .MaximumLength(256);

            RuleFor(x => x.Email).NotEmpty()
                .EmailAddress();

            RuleFor(x => x.PhoneNumber).NotEmpty()
                .MaximumLength(64);

            RuleFor(x => x.DepartmentTeamId).NotNull();

            RuleFor(x => x.EmploymentType).NotNull();
        }
    }
}
