using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.AddRemoveUserToProject
{
    public class ProjectAddOrRemoveUserCommandValidator : AbstractValidator<ProjectAddOrRemoveUserCommand>
    {
        public ProjectAddOrRemoveUserCommandValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.ApplicationUserId)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.IncludeUser)
                .NotNull();
        }
    }
}
