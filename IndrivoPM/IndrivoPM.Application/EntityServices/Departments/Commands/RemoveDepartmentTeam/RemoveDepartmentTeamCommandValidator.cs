using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.RemoveDepartmentTeam
{
    public class RemoveDepartmentTeamCommandValidator : AbstractValidator<RemoveDepartmentTeamCommand>
    {
        public RemoveDepartmentTeamCommandValidator()
        {

            RuleFor(x => x.DepartmentTeamIds).NotNull();
        }
    }
}
