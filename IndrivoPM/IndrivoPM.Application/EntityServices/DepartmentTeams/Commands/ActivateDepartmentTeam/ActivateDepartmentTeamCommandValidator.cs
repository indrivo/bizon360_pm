using FluentValidation;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.ActivateDepartmentTeam
{
    public class ActivateDepartmentTeamCommandValidator : AbstractValidator<ActivateDepartmentTeamCommand>
    {
        public ActivateDepartmentTeamCommandValidator()
        {
            RuleFor(x => x.Ids).NotEmpty().NotNull();

        }
    }
}
