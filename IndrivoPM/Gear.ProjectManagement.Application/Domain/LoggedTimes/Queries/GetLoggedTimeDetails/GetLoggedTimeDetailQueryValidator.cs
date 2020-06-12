using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetLoggedTimeDetails
{
    public class GetLoggedTimeDetailQueryValidator : AbstractValidator<GetLoggedTimeDetailQuery>
    {
        public GetLoggedTimeDetailQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }
}
