using FluentValidation;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Commands.CreateBusinessUnit
{
    public class CreateBusinessUnitCommandValidator : AbstractValidator<CreateBusinessUnitCommand>
    {
        public CreateBusinessUnitCommandValidator()
        {
            RuleFor(x => x.Name).MaximumLength(256)
                .NotEmpty();

            RuleFor(x => x.Description).MaximumLength(1000);
        }
    }
}
