using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
    {
        public DeleteProjectCommandValidator()
        {
            RuleFor(p => p.Id).NotNull().NotEmpty();
        }
    }
}