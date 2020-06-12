using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetGeneralSprintListByProjectReport
{
    public class GetGeneralSprintListByProjectReportQueryValidator : AbstractValidator<GetGeneralSprintListByProjectReportQuery>
    {
        public GetGeneralSprintListByProjectReportQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }
}
