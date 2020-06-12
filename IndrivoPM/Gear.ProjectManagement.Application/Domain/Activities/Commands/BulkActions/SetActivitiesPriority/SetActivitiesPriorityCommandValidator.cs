using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.SetActivitiesPriority
{
    public class SetActivitiesPriorityCommandValidator : AbstractValidator<SetActivitiesPriorityCommand>
    {
        public SetActivitiesPriorityCommandValidator()
        {
            RuleFor(a => a.ActivitiesById).NotEmpty();

            RuleFor(a => a.Priority).NotNull().NotEmpty();
        }
    }
}
