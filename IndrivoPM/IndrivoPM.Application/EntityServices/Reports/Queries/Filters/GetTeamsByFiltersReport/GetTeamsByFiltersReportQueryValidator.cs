using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport
{
    public class GetTeamsByFiltersReportQueryValidator : AbstractValidator<GetTeamsByFiltersReportQuery>
    {
        public GetTeamsByFiltersReportQueryValidator()
        {
            RuleFor(x => x.EmployeeIds)
                .NotEmpty();

            RuleFor(x => x.ActivityStatuses)
                .NotEmpty();

            RuleFor(x => x.ActivityTypeIds)
                .NotEmpty();

            RuleFor(x => x.ProjectIds)
                .NotEmpty();

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(xx => xx.DueDate).WithMessage("The starting date can't be greater than the due one.");
        }
    }
}
