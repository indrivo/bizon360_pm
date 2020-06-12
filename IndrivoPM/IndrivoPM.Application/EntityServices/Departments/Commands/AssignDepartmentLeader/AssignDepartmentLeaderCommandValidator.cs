using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.AssignDepartmentLeader
{
    public class AssignDepartmentLeaderCommandValidator : AbstractValidator<AssignDepartmentLeaderCommand>
    {
        public AssignDepartmentLeaderCommandValidator()
        {
            RuleFor(x => x.Id).NotNull();

            RuleFor(x => x.DepartmentLeadId).NotNull();
        }
    }
}
