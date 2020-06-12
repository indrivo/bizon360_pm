using FluentValidation;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamDetails
{
    public class DepartmentTeamDetailQueryValidator:AbstractValidator<DepartmentTeamDetailQuery>
    {
        public DepartmentTeamDetailQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
