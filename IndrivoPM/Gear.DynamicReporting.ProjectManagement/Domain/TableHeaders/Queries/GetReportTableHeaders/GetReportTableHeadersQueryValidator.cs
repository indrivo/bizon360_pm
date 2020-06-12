using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetReportTableHeaders
{
    public class GetReportTableHeadersQueryValidator : AbstractValidator<GetReportTableHeadersQuery>
    {
        public GetReportTableHeadersQueryValidator()
        {
            RuleFor(x => x.ReportId)
                .NotEmpty();
        }
    }
}
