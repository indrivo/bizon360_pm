using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Queries.GetUserReportProjectStatuses
{
    public class GetUserReportProjectStatusesQueryValidator : AbstractValidator<GetUserReportProjectStatusesQuery>
    {
        public GetUserReportProjectStatusesQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.ReportId)
                .NotEmpty();
        }
    }
}
