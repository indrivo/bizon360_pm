using FluentValidation;
using Microsoft.EntityFrameworkCore.Internal;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByTypeReport
{
    public class GetTasksByTypeReportQueryValidator : AbstractValidator<GetTasksByTypeReportQuery>
    {
        public GetTasksByTypeReportQueryValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty();

            //RuleFor(x => x)
            //    .Must(x => x.ActivityTypes.Any() || x.ShowAllData);
        }
    }
}
