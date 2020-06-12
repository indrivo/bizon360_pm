using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityDetails
{
    public class GetActivityDetailQueryValidator : AbstractValidator<GetActivityDetailQuery>
    {
        public GetActivityDetailQueryValidator()
        {
            RuleFor(a => a.Id).NotEmpty();
        }
    }
}
