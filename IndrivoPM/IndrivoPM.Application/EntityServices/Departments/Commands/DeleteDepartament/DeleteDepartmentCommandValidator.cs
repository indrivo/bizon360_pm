using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.DeleteDepartament
{
    public class DeleteDepartmentCommandValidator:AbstractValidator<DeleteDepartmentCommand>
    {
        public DeleteDepartmentCommandValidator()
        {
            RuleFor(x => x.Ids).NotEmpty().NotNull();
        }
    }
}
