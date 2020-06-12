using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.ShortcutActions.UpdateProjectStatus
{
    public class UpdateProjectStatusCommandValidator : AbstractValidator<UpdateProjectStatusCommand>
    {
        public UpdateProjectStatusCommandValidator()
        {
            RuleFor(p => p.Id).NotNull().NotEmpty();

            RuleFor(p => p.Status).NotNull().NotEmpty();
        }
    }
}
