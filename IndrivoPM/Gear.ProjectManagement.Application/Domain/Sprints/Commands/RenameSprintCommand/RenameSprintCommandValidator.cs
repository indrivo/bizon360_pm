using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Commands.RenameSprintCommand
{
    public class RenameSprintCommandValidator : AbstractValidator<RenameSprintCommand>
    {
        public RenameSprintCommandValidator()
        {
            RuleFor(a => a.Id).NotNull().NotEmpty();

            RuleFor(a => a.Name)
                .NotEmpty()
                .MaximumLength(256);
        }
    }
}
