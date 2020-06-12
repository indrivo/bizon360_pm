using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.ShortcutActions.EditProjectMembers
{
    public class EditProjectMembersCommandValidator : AbstractValidator<EditProjectMembersCommand>
    {
        public EditProjectMembersCommandValidator()
        {
            RuleFor(c => c.Id).NotNull().NotEmpty();
        }
    }
}
