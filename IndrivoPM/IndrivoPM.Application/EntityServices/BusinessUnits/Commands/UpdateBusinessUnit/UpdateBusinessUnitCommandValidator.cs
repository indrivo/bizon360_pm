using FluentValidation;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Commands.UpdateBusinessUnit
{
    public class UpdateBusinessUnitCommandValidator : AbstractValidator<UpdateBusinessUnitCommand>
    {
        public UpdateBusinessUnitCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Name).MaximumLength(256)
                .NotEmpty();

            RuleFor(x => x.Description).MaximumLength(1000);
        }
    }
}
