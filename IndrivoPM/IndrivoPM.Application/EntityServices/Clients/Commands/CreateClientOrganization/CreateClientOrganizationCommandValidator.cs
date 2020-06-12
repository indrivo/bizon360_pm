using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Clients.Commands.CreateClientOrganization
{
    public class CreateClientOrganizationCommandValidator : AbstractValidator<CreateClientOrganizationCommand>
    {
        public CreateClientOrganizationCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Name)
                .MaximumLength(100)
                .NotNull()
                .NotEmpty();
        }
    }
}
