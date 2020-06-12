using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityProgress
{
    public class GetActivityProgressQueryValidator : AbstractValidator<GetActivityProgressQuery>
    {
        public GetActivityProgressQueryValidator()
        {
            RuleFor(r => r.Id).NotNull().NotEmpty();
        }
    }
}
