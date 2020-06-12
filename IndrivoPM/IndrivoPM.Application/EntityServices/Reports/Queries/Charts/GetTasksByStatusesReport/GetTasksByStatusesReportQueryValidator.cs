using System.Linq;
using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByStatusesReport
{
    public class GetTasksByStatusesReportQueryValidator : AbstractValidator<GetTasksByStatusesReportQuery>
    {
        public GetTasksByStatusesReportQueryValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty();

            //RuleFor(x => x)
            //    .Must(x => x.Statuses.Any() || x.ShowAllData);
        }
    }
}
