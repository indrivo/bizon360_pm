using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.ProjectSstp
{
    internal class ProjectSstpCommandValidator : AbstractValidator<ProjectSstpCommand>
    {
        public ProjectSstpCommandValidator()
        {
            RuleFor(x => x.ProjectId).NotNull().NotEmpty();
        }
    }
}
