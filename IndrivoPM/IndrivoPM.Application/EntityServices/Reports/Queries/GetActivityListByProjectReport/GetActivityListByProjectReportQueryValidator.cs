using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetActivityListByProjectReport
{
    public class GetActivityListByProjectReportQueryValidator  : AbstractValidator<GetActivityListByProjectReportQuery>
    {
        public GetActivityListByProjectReportQueryValidator()
        {
            RuleFor(x => x.ProjectId).NotNull().NotEmpty();

            RuleFor(x => x.ActivityListIds).NotNull().NotEmpty();
        }
    }
}
