using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByPriorityReport
{
    public class GetTasksByPriorityReportQueryValidator : AbstractValidator<GetTasksByPriorityReportQuery>
    {
        public GetTasksByPriorityReportQueryValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty();
        }
    }
}
