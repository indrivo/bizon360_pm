using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectGroupsGeneralReport
{
    public class GetProjectGroupsGeneralReportQueryValidator : AbstractValidator<GetProjectGroupsGeneralReportQuery>
    {
        public GetProjectGroupsGeneralReportQueryValidator()
        {
            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(xx => xx.DueDate).WithMessage("The starting date can't be greater than the due one.");
        }
    }
}
