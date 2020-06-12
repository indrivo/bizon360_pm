using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
    {
        public UpdateDepartmentCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Name).MaximumLength(256)
                .NotEmpty();

            RuleFor(x => x.Description).MaximumLength(1000);


        }
    }
}
