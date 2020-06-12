using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.SetActivitiesStatus
{
    public class SetActivitiesStatusCommandValidator : AbstractValidator<SetActivitiesStatusCommand>
    {
        public SetActivitiesStatusCommandValidator()
        {
            RuleFor(a => a.ActivitiesById).NotEmpty();

            RuleFor(a => a.Status).NotNull().NotEmpty();
        }
    }
}
