using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetLoggedTimeList
{
    public class GetLoggedTimeListQueryValidator:AbstractValidator<GetLoggedTimeListQuery>
    {
        public GetLoggedTimeListQueryValidator()
        {
            RuleFor(x => x.ActivityId)
                .NotEmpty()
                .NotNull();
        }
    }
}
