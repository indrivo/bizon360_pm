using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Queries.GetActivityTypeDetails
{
    public class GetActivityTypeDetailQueryValidator: AbstractValidator<GetActivityTypeDetailQuery>
    {
        public GetActivityTypeDetailQueryValidator()
        {
            RuleFor(pg => pg.Id).NotEmpty();
        }
    }
}
