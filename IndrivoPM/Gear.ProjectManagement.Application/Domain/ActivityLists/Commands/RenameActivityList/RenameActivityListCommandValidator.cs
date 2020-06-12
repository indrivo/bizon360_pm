using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.RenameActivityList
{
    public class RenameActivityListCommandValidator : AbstractValidator<RenameActivityListCommand>
    {
        public RenameActivityListCommandValidator()
        {
            RuleFor(a => a.Id).NotNull().NotEmpty();

            RuleFor(a => a.Name)
                .NotEmpty()
                .MaximumLength(256);
        }
    }
}
