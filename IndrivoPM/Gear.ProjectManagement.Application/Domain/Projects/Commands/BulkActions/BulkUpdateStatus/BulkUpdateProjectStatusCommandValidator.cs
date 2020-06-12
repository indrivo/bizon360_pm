using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.BulkActions.BulkUpdateStatus
{
    public class BulkUpdateProjectStatusCommandValidator : AbstractValidator<BulkUpdateProjectStatusCommand>
    {
        public BulkUpdateProjectStatusCommandValidator()
        {
            RuleFor(request => request.Projects).NotEmpty();

            RuleFor(request => request.Status).NotNull().NotEmpty();
        }
    }
}
