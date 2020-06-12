using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport
{
    public class GetProjectsByFiltersReportQueryValidator : AbstractValidator<GetProjectsByFiltersReportQuery>
    {
        public GetProjectsByFiltersReportQueryValidator()
        {
            RuleFor(x => x.ProjectIds)
                .NotEmpty();

            RuleFor(x => x.ActivityStatuses)
                .NotEmpty();

            RuleFor(x => x.ActivityTypes)
                .NotEmpty();

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(xx => xx.DueDate).WithMessage("The starting date can't be greater than the due one.");
        }
    }
}
