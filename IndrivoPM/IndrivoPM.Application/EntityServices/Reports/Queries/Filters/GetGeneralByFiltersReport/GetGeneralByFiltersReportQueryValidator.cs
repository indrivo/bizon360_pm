using FluentValidation;
using Gear.Manager.Core.EntityServices.Reports.Enums;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport
{
    public class GetGeneralByFiltersReportQueryValidator : AbstractValidator<GetGeneralByFiltersReportQuery>
    {
        public GetGeneralByFiltersReportQueryValidator()
        {
            RuleFor(x => x.GroupsIds)
                .NotEmpty();

            RuleFor(x => x.ProjectsIds)
                .NotEmpty();

            RuleFor(x => x.TeamsIds)
                .NotEmpty();

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate)
                .WithMessage("The starting date can't be greater than the due one.");
        }
    }
}
