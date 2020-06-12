using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.ShortcutActions.SetProjectPriority
{
    public class SetProjectPriorityCommandValidator : AbstractValidator<SetProjectPriorityCommand>
    {
        public SetProjectPriorityCommandValidator()
        {
            RuleFor(request => request.Id).NotNull().NotEmpty();

            RuleFor(request => request.Priority).NotNull().NotEmpty();
        }
    }
}
