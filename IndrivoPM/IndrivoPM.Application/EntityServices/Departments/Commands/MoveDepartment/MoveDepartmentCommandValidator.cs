using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.MoveDepartment
{
    public class MoveDepartmentCommandValidator : AbstractValidator<MoveDepartmentCommand>
    {
        public MoveDepartmentCommandValidator()
        {
            RuleFor(x => x.DepartmentIds).NotNull().NotEmpty();

        }
    }
}
