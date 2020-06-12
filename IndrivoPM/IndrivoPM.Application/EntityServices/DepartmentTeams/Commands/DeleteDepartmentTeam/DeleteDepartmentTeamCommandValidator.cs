using FluentValidation;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.DeleteDepartmentTeam
{
    public class DeleteDepartmentTeamCommandValidator:AbstractValidator<DeleteDepartmentTeamCommand>
    {
        public DeleteDepartmentTeamCommandValidator()
        {
            RuleFor(x => x.Ids).NotEmpty();
        }
    }
}
