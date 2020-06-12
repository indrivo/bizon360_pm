using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityDto
{
    public class GetActivityDtoQueryValidator : AbstractValidator<GetActivityDtoQuery>
    {
        public GetActivityDtoQueryValidator()
        {
            RuleFor(a => a.Id).NotEmpty();
        }
    }
}
