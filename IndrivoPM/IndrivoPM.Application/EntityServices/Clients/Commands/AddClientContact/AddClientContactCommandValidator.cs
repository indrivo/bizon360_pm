using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Clients.Commands.AddClientContact
{
    public class AddClientContactCommandValidator : AbstractValidator<AddClientContactCommand>
    {
        public AddClientContactCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.OrganizationId)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.ContactInfo)
                .NotNull();
        }
    }
}
