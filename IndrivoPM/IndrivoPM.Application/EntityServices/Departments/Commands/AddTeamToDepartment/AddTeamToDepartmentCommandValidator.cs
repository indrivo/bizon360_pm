using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.AddTeamToDepartment
{
    public class AddTeamToDepartmentCommandValidator : AbstractValidator<AddTeamToDepartmentCommand>
    {
        public AddTeamToDepartmentCommandValidator()
        {
            RuleFor(x => x.DepartmentId).NotNull();
        }
    }
}
