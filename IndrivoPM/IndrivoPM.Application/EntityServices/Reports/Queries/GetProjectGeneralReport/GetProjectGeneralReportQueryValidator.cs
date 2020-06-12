using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectGeneralReport
{
    public class GetProjectGeneralReportQueryValidator : AbstractValidator<GetProjectGeneralReportQuery>
    {
        public GetProjectGeneralReportQueryValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty();

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(xx => xx.DueDate).WithMessage("The starting date can't be greater than the due one.");
        }
    }
}
