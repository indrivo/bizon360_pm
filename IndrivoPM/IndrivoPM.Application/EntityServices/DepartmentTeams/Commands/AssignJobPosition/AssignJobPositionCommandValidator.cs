using FluentValidation;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Commands.AssignJobPosition
{
    public class AssignJobPositionCommandValidator : AbstractValidator<AssignJobPositionCommand>
    {
        public AssignJobPositionCommandValidator()
        {
            RuleFor(x => x.Id).NotNull();

            RuleFor(x => x.JobPositionIds).NotNull();
        }
    }
}
