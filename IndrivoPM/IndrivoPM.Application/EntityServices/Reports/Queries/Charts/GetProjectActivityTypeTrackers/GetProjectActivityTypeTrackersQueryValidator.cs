using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetProjectActivityTypeTrackers
{
    class GetProjectActivityTypeTrackersQueryValidator : AbstractValidator<GetProjectActivityTypeTrackersQuery>
    {
        public GetProjectActivityTypeTrackersQueryValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty();

            RuleFor(x => x.ActivityTypeId)
                .NotEmpty();
        }
    }
}
