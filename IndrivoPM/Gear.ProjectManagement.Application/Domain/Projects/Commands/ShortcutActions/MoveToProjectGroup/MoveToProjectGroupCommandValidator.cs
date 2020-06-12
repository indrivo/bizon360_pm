using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.ShortcutActions.MoveToProjectGroup
{
    public class MoveToProjectGroupCommandValidator : AbstractValidator<MoveToProjectGroupCommand>
    {
        public MoveToProjectGroupCommandValidator()
        {
            RuleFor(p => p.Id).NotNull().NotEmpty();

            RuleFor(p => p.ProjectGroupId).NotNull().NotEmpty();
        }
    }
}
