using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.ShortcutActions.UpdatePriority
{
    public class UpdatePriorityCommandValidator : AbstractValidator<UpdatePriorityCommand>
    {
        public UpdatePriorityCommandValidator()
        {
            RuleFor(c => c.Id).NotNull().NotEmpty();

            RuleFor(c => c.Priority).NotNull().NotEmpty();
        }
    }
}
