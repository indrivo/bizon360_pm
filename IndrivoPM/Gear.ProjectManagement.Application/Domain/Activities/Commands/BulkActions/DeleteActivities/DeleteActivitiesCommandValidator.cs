using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.DeleteActivities
{
    public class DeleteActivitiesCommandValidator : AbstractValidator<DeleteActivitiesCommand>
    {
        public DeleteActivitiesCommandValidator()
        {
            RuleFor(a => a.ActivitiesById).NotEmpty();
        }
    }
}
