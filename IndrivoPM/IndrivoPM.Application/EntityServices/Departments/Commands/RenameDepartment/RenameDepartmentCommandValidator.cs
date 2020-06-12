using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.RenameDepartment
{
    public class RenameDepartmentCommandValidator : AbstractValidator<RenameDepartmentCommand>
    {
        public RenameDepartmentCommandValidator()
        {
            RuleFor(x => x.Id).NotNull();

            RuleFor(x => x.Name).MaximumLength(256)
                .NotEmpty();
        }
    }
}
