using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.DeleteActivityType
{
    public class DeleteActivityTypeCommandValidator:AbstractValidator<DeleteActivityTypeCommand>
    {
        public DeleteActivityTypeCommandValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
