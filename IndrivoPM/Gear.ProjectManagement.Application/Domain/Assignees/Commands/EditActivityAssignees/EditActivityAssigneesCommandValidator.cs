using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Assignees.Commands.EditActivityAssignees
{
    public class EditActivityAssigneesCommandValidator : AbstractValidator<EditActivityAssigneesCommand>
    {
        public EditActivityAssigneesCommandValidator()
        {
            RuleFor(c => c.Id).NotNull().NotEmpty();
        }
    }
}
