using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.Search.SearchActivitiesByName
{
    public class SearchActivitiesByNameValidator: AbstractValidator<SearchActivitiesByNameQuery>
    {
        public SearchActivitiesByNameValidator()
        {
            RuleFor(request => request.ProjectId).NotNull().NotEmpty();

            RuleFor(request => request.SearchValue).NotNull().NotEmpty();
        }
    }
}
