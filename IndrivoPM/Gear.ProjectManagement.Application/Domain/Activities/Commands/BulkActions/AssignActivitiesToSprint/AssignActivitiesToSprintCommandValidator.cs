using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.AssignActivitiesToSprint
{
    public class AssignActivitiesToSprintCommandValidator : AbstractValidator<AssignActivitiesToSprintCommand>
    {
        public AssignActivitiesToSprintCommandValidator()
        {
            RuleFor(a => a.ActivitiesById).NotEmpty();

            RuleFor(a => a.SprintId).NotNull().NotEmpty();
        }
    }
}
