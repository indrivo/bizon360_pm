using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetUserReportList
{
    public class GetUserReportListQueryValidator : AbstractValidator<GetUserReportListQuery>
    {
        public GetUserReportListQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
