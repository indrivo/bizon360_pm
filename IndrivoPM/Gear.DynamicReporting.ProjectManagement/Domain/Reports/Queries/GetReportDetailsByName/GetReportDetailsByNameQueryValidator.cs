using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Queries.GetReportDetailsByName
{
    public class GetReportDetailsByNameQueryValidator : AbstractValidator<GetReportDetailsByNameQuery>
    {
        public GetReportDetailsByNameQueryValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty();
        }
    }
}
