using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.UpdateOrderActivityType
{
    public class UpdateOrderActivityTypeCommandValidator : AbstractValidator<UpdateOrderActivityTypeCommand>
    {
        public UpdateOrderActivityTypeCommandValidator()
        {
            RuleFor(x => x.ActivityTypeIds).NotNull().NotEmpty();
        }
    }
}
