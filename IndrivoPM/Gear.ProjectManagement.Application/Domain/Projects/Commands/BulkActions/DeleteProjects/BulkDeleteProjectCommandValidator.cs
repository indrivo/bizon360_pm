using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.BulkActions.DeleteProjects
{
    public class BulkDeleteProjectCommandValidator : AbstractValidator<BulkDeleteProjectCommand>
    {
        public BulkDeleteProjectCommandValidator()
        {
            RuleFor(request => request.Projects).NotEmpty();
        }
    }
}
