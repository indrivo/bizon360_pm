using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.RenameActivityType
{
    public class RenameActivityTypeCommandValidator : AbstractValidator<RenameActivityTypeCommand>
    {
        public RenameActivityTypeCommandValidator()
        {
            RuleFor(x => x.Id).NotNull();

            RuleFor(x => x.Name).MaximumLength(256)
                .NotEmpty();
        }
    }
}
