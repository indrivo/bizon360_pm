using FluentValidation;
using Gear.Domain.ReportEntities.Enums;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Queries.GetUserReportGuidList
{
    public class GetUserReportGuidListQueryValidator : AbstractValidator<GetUserReportGuidListQuery>
    {
        public GetUserReportGuidListQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.ReportId)
                .NotEmpty();

            RuleFor(x => x.FilterType)
                .Must(x => x == FilterType.ActivityIds ||
                           x == FilterType.ActivityListIds ||
                           x == FilterType.ActivityTypeIds ||
                           x == FilterType.EmployeeIds ||
                           x == FilterType.ProjectGroupIds ||
                           x == FilterType.ProjectIds ||
                           x == FilterType.SprintIds);
        }
    }
}
