using FluentValidation;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.AssignDepartmentTeamLeader
{
    public class AssignDepartmentTeamLeaderCommandValidator : AbstractValidator<AssignDepartmentTeamLeaderCommand>
    {
        public AssignDepartmentTeamLeaderCommandValidator()
        {
            RuleFor(x => x.Id).NotNull();

            RuleFor(x => x.DepartmentTeamLeadId).NotNull();
        }
    }
}
