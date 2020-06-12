using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport
{
    public class GetEmployeeFiltersReportQueryValidator : AbstractValidator<GetEmployeeFiltersReportQuery>
    {
        public GetEmployeeFiltersReportQueryValidator()
        {
            RuleFor(a => a.UserIds)
                .NotEmpty();

            RuleFor(a => a.ProjectIds)
                .NotEmpty();

            RuleFor(a => a.StartDate)
                .LessThanOrEqualTo(a => a.DueDate).WithMessage("The starting date can't be greater than the ending one.");
        }
    }
}
