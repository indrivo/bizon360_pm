using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.UpdateOrderDepartment
{
    public class UpdateOrderDepartmentCommandValidator : AbstractValidator<UpdateOrderDepartmentCommand>
    {
        public UpdateOrderDepartmentCommandValidator()
        {
            RuleFor(x => x.DepartmentIds).NotNull();
        }
    }
}
