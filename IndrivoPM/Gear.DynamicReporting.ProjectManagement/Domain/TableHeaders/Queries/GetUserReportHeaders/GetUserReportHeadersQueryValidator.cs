using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetUserReportHeaders
{
    public class GetUserReportHeadersQueryValidator : AbstractValidator<GetUserReportHeadersQuery>
    {
        public GetUserReportHeadersQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.ReportId)
                .NotEmpty();
        }
    }
}
