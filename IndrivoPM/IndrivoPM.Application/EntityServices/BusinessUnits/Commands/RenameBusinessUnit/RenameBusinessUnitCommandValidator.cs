using FluentValidation;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Commands.RenameBusinessUnit
{
    public class RenameBusinessUnitCommandValidator : AbstractValidator<RenameBusinessUnitCommand>
    {
        public RenameBusinessUnitCommandValidator()
        {
            RuleFor(x => x.Id).NotNull();

            RuleFor(x => x.Name).MaximumLength(256)
                .NotEmpty();
        }
    }
}
