using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchActivitiesByActivityList
{
    public class SearchActivitiesByActivityListQueryValidator : AbstractValidator<SearchActivitiesByActivityListQuery>
    {
        public SearchActivitiesByActivityListQueryValidator()
        {
            RuleFor(request => request.ProjectId).NotNull().NotEmpty();

            RuleFor(request => request.SearchValue).NotNull().NotEmpty();
        }
    }
}
