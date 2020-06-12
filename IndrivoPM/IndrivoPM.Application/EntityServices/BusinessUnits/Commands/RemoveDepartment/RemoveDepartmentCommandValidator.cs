using FluentValidation;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Commands.RemoveDepartment
{
    public class RemoveDepartmentCommandValidator : AbstractValidator<RemoveDepartmentCommand>
    {
        public RemoveDepartmentCommandValidator()
        {

            RuleFor(x => x.DepartmentIds).NotNull().NotEmpty();
        }
    }
}
