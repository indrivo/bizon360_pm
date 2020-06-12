using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.MarkActivitiesAsCompleted
{
    public class MarkActivitiesAsCompletedCommandValidator : AbstractValidator<MarkActivitiesAsCompletedCommand>
    {
        public MarkActivitiesAsCompletedCommandValidator()
        {
            RuleFor(a => a.ActivitiesById).NotEmpty();
        }
    }
}
