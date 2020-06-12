using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetTableHeaderDetailsByName
{
    public class GetTableHeaderDetailsByNameQueryValidator : AbstractValidator<GetTableHeaderDetailsByNameQuery>
    {
        public GetTableHeaderDetailsByNameQueryValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty();
        }
    }
}
