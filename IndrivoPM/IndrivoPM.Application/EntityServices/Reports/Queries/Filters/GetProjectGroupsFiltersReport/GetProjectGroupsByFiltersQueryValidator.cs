using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport
{
    public class GetProjectGroupsByFiltersQueryValidator : AbstractValidator<GetProjectGroupsByFiltersQuery>
    {
        public GetProjectGroupsByFiltersQueryValidator()
        {
            RuleFor(x => x.ActivityTypeIds)
                .NotEmpty();

            RuleFor(x => x.AssigneeIds)
                .NotEmpty();

            RuleFor(x => x.ProjectStatuses)
                .NotEmpty();

            RuleFor(x => x.DueDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("The starting date can't be greater than the due one.");
        }
    }
}
