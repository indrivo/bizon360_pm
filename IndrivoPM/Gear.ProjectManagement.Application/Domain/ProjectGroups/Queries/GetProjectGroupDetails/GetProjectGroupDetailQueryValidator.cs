using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupDetails
{
    public class GetProjectGroupDetailQueryValidator : AbstractValidator<GetProjectGroupDetailQuery>
    {
        public GetProjectGroupDetailQueryValidator()
        {
            RuleFor(pg => pg.Id).NotNull().NotEmpty();
        }
    }
}
