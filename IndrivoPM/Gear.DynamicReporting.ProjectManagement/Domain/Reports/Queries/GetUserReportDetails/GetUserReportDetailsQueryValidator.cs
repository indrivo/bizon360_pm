using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetUserReportDetails
{
    public class GetUserReportDetailsQueryValidator : AbstractValidator<GetUserReportDetailsQuery>
    {
        public GetUserReportDetailsQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.ReportId)
                .NotEmpty();
        }
    }
}
