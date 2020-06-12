using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.BulkActions.BulkMoveToProjectGroup
{
    public class BulkMoveToProjectGroupCommandValidator : AbstractValidator<BulkMoveToProjectGroupCommand>
    {
        public BulkMoveToProjectGroupCommandValidator()
        {
            RuleFor(request => request.Projects).NotEmpty();

            RuleFor(request => request.ProjectGroupId).NotNull().NotEmpty();
        }
    }
}
