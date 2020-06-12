using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.ShortcutActions.MoveToSprint
{
    public class MoveToSprintCommandValidator : AbstractValidator<MoveToSprintCommand>
    {
        public MoveToSprintCommandValidator()
        {
            RuleFor(c => c.Id).NotNull().NotEmpty();

            RuleFor(c => c.SprintId).NotNull().NotEmpty();
        }
    }
}
