using FluentValidation;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.AssignUser
{
    public class AssignUserCommandValidator : AbstractValidator<AssignUserCommand>
    {
        public AssignUserCommandValidator()
        {
            RuleFor(x => x.DepartmentTeamId).NotNull().NotEmpty();
        }
    }
}
