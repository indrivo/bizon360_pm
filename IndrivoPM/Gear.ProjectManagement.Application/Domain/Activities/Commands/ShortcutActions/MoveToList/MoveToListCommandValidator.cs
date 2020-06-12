using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.ShortcutActions.MoveToList
{
    public class MoveToListCommandValidator : AbstractValidator<MoveToListCommand>
    {
        public MoveToListCommandValidator()
        {
            RuleFor(c => c.Id).NotNull().NotEmpty();

            RuleFor(c => c.ActivityListId).NotNull().NotEmpty();
        }
    }
}
