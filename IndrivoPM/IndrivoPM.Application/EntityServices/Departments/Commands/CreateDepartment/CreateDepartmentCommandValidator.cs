using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.CreateDepartment
{
   public class CreateDepartmentCommandValidator:AbstractValidator<CreateDepartmentCommand>
    {
        public CreateDepartmentCommandValidator()
        {
            RuleFor(x => x.Name).MaximumLength(256)
                .NotEmpty();

            RuleFor(x => x.Description).MaximumLength(1000);

        }
    }
}
