using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.RemoveActivitiesFromSprint
{
    public class RemoveActivitiesFromSprintCommandValidator : AbstractValidator<RemoveActivitiesFromSprintCommand>
    {
        public RemoveActivitiesFromSprintCommandValidator()
        {
            RuleFor(a => a.ActivitiesById).NotEmpty();
        }
    }
}
