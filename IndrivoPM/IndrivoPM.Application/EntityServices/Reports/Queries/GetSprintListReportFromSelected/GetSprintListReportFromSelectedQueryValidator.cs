using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetSprintListReportFromSelected
{
    public class GetSprintListReportFromSelectedQueryValidator : AbstractValidator<GetSprintListReportFromSelectedQuery>
    {
        public GetSprintListReportFromSelectedQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.SprintIds)
                .NotEmpty();
        }
    }
}
