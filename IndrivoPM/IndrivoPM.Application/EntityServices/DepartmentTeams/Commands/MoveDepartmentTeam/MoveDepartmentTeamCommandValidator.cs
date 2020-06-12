using FluentValidation;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.MoveDepartmentTeam
{
    public class MoveDepartmentTeamCommandValidator : AbstractValidator<MoveDepartmentTeamCommand>
    {
        public MoveDepartmentTeamCommandValidator()
        {
            RuleFor(x => x.DepartmentId).NotNull();

            RuleFor(x => x.DepartmentTeamIds).NotNull();

        }
    }
}
