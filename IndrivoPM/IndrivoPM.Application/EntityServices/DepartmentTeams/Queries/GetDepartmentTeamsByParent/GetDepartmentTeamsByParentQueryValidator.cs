using FluentValidation;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamsByParent
{
    public class GetDepartmentTeamsByParentQueryValidator : AbstractValidator<GetDepartmentTeamsByParentQuery>
    {
        public GetDepartmentTeamsByParentQueryValidator()
        {
            RuleFor(x => x.DepartmentId).NotNull();
        }
    }
}

