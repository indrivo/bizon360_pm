using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Queries.GetSprintListQuery
{
    public class GetSprintListQueryValidator : AbstractValidator<GetSprintListQuery>
    {
        public GetSprintListQueryValidator()
        {
            RuleFor(request => request.ProjectId).NotNull().NotEmpty();
        }
    }
}
