using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.ShortcutActions.RemoveFromSprint
{
    public class RemoveFromSprintCommandValidator : AbstractValidator<RemoveFromSprintCommand>
    {
        public RemoveFromSprintCommandValidator()
        {
            RuleFor(c => c.Id).NotNull().NotEmpty();
        }
    }
}
