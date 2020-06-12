using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Queries.GetTrackerTypeDetails
{
    public class GetTrackerTypeDetailQueryValidator:AbstractValidator<GetTrackerTypeDetailQuery>
    {
        public GetTrackerTypeDetailQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
