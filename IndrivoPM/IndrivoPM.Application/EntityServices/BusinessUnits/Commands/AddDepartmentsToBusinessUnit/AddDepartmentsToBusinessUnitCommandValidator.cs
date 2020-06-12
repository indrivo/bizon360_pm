using FluentValidation;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Commands.AddDepartmentsToBusinessUnit
{
    public class AddDepartmentsToBusinessUnitCommandValidator : AbstractValidator<AddDepartmentsToBusinessUnitCommand>
    {
        public AddDepartmentsToBusinessUnitCommandValidator()
        {
            RuleFor(x => x.BusinessUnitId).NotNull();
        }
    }
}
