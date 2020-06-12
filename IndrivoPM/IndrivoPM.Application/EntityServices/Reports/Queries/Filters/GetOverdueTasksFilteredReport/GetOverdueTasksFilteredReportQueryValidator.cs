using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport
{
    public class GetOverdueTasksFilteredReportQueryValidator : AbstractValidator<GetOverdueTasksFilteredReportQuery>
    {
        public GetOverdueTasksFilteredReportQueryValidator()
        {
            RuleFor(x => x.ProjectIds)
                .NotEmpty();

            RuleFor(x => x.AssigneeIds)
                .NotEmpty();

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(x => x.DueDate);
        }
    }
}
