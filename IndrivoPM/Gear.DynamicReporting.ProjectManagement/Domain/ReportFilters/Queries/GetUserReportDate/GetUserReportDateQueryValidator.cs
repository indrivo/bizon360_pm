using FluentValidation;
using Gear.Domain.ReportEntities.Enums;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Queries.GetUserReportDate
{
    public class GetUserReportDateQueryValidator : AbstractValidator<GetUserReportDateQuery>
    {
        public GetUserReportDateQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.ReportId)
                .NotEmpty();

            RuleFor(x => x.FilterType)
                .Must(x => x == FilterType.StartDate || x == FilterType.DueDate);
        }
    }
}
