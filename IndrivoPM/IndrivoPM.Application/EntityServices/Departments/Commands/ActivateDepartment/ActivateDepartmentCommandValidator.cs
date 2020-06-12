using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.ActivateDepartment
{
    public class ActivateDepartmentCommandValidator:AbstractValidator<ActivateDepartmentCommand>
    {
        public ActivateDepartmentCommandValidator()
        {
            RuleFor(x => x.Ids).NotEmpty().NotNull();

        }
    }
}
