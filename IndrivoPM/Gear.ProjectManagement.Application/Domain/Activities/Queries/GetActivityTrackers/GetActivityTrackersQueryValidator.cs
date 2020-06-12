using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityTrackers
{
    public class GetActivityTrackersQueryValidator : AbstractValidator<GetActivityTrackersQuery>
    {
        public GetActivityTrackersQueryValidator()
        {
            RuleFor(request => request.Id).NotNull().NotEmpty();
        }
    }
}
