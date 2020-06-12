using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.BulkActions.BulkSetPriority
{
    public class BulkSetProjectPriorityCommandValidator : AbstractValidator<BulkSetProjectPriorityCommand>
    {
        public BulkSetProjectPriorityCommandValidator()
        {
            RuleFor(request => request.Projects).NotEmpty();

            RuleFor(request => request.Priority).NotNull().NotEmpty();
        }
    }
}
