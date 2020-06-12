using FluentValidation;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.UpdateDepartmentTeam
{
    public class UpdateDepartmentTeamCommandValidator:AbstractValidator<UpdateDepartmentTeamCommand>
    {
        public UpdateDepartmentTeamCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Name).MaximumLength(256)
                .NotEmpty();
        }
    }
}
