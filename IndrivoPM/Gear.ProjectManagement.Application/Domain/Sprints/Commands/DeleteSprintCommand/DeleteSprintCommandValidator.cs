using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Commands.DeleteSprintCommand
{
    public class DeleteSprintCommandValidator : AbstractValidator<DeleteSprintCommand>
    {
        public DeleteSprintCommandValidator()
        {
            RuleFor(s => s.Id).NotEmpty();
        }
    }
}
