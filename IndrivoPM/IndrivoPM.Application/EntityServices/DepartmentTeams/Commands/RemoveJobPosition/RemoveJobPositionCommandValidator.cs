using FluentValidation;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.RemoveJobPosition
{
    public class RemoveJobPositionCommandValidator:AbstractValidator<RemoveJobPositionCommand>
    {
        public RemoveJobPositionCommandValidator()
        {
            RuleFor(x => x.DepartmentTeamId).NotNull();

            RuleFor(x => x.JobPositionIds).NotNull();
        }
    }
}
