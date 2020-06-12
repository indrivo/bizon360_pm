using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.BulkActions.MoveActivitiesToList
{
    public class MoveActivitiesToListCommandValidator : AbstractValidator<MoveActivitiesToListCommand>
    {
        public MoveActivitiesToListCommandValidator()
        {
            RuleFor(a => a.ActivityListId).NotEmpty();

            RuleFor(a => a.ActivityListId).NotNull().NotEmpty();
        }
    }
}
