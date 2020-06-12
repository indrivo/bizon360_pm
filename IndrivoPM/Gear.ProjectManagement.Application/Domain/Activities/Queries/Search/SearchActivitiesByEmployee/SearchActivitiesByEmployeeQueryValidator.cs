using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchActivitiesByEmployee
{
    public class SearchActivitiesByEmployeeQueryValidator : AbstractValidator<SearchActivitiesByEmployeeQuery>
    {
        public SearchActivitiesByEmployeeQueryValidator()
        {
            RuleFor(request => request.ProjectId).NotNull().NotEmpty();

            RuleFor(request => request.SearchValue).NotNull().NotEmpty();
        }
    }
}
