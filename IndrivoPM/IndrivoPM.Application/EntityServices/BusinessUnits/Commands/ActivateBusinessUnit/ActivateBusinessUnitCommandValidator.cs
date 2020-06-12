using FluentValidation;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Commands.ActivateBusinessUnit
{
    public class ActivateBusinessUnitCommandValidator : AbstractValidator<ActivateBusinessUnitCommand>
    {
        public ActivateBusinessUnitCommandValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
