using FluentValidation;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Commands.AssignBusinessUnitLeader
{
    public class AssignBusinessUnitLeaderCommandValidator : AbstractValidator<AssignBusinessUnitLeaderCommand>
    {
        public AssignBusinessUnitLeaderCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
