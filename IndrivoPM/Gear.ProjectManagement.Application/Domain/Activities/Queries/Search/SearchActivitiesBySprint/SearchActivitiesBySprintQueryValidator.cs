using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchActivitiesBySprint
{
    public class SearchActivitiesBySprintQueryValidator : AbstractValidator<SearchActivitiesBySprintQuery>
    {
        public SearchActivitiesBySprintQueryValidator()
        {
            RuleFor(request => request.ProjectId).NotNull().NotEmpty();

            RuleFor(request => request.SearchValue).NotNull().NotEmpty();
        }
    }
}
