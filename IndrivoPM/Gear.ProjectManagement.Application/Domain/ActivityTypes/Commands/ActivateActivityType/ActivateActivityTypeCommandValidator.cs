using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.ActivateActivityType
{
    public class ActivateActivityTypeCommandValidator : AbstractValidator<ActivateActivityTypeCommand >
    {
        public ActivateActivityTypeCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
