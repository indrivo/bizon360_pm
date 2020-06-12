using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Querries.GetActivityLists
{
    public class GetActivityListsQueryValidator : AbstractValidator<GetActivityListsQuery>
    {
        public GetActivityListsQueryValidator()
        {
            RuleFor(request => request.ProjectId).NotNull().NotEmpty();
        }
    }
}
