using FluentValidation;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.UpdateOrderDepartmentTeam
{
    public class UpdateOrderDepartmentTeamCommandValidator : AbstractValidator<UpdateOrderDepartmentTeamCommand>
    {
        public UpdateOrderDepartmentTeamCommandValidator()
        {
            RuleFor(x => x.DepartmentTeamIds).NotNull();
        }
    }
}
