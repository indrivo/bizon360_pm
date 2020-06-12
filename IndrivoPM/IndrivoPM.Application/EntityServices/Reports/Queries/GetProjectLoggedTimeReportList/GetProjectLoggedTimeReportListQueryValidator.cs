using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectLoggedTimeReportList
{
    public class GetProjectLoggedTimeReportListQueryValidator : AbstractValidator<GetProjectLoggedTimeReportListQuery>
    {
        public GetProjectLoggedTimeReportListQueryValidator()
        {
            RuleFor(x => x.ProjectIds)
                .NotEmpty().NotNull();

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(xx => xx.DueDate);
        }
    }
}
