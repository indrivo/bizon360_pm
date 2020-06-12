using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetReportDetailsById
{
    public class GetReportDetailsByIdQueryValidator : AbstractValidator<GetReportDetailsByIdQuery>
    {
        public GetReportDetailsByIdQueryValidator()
        {
            RuleFor(x => x.ReportId)
                .NotEmpty();
        }
    }
}
