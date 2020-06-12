using FluentValidation;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Commands.DeleteBusinessUnit
{
    public class DeleteBusinessUnitCommandValidator : AbstractValidator<DeleteBusinessUnitCommand>
    {
        public DeleteBusinessUnitCommandValidator()
        {
            //TODO:Remake: dont forget to check if id is not empty
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
