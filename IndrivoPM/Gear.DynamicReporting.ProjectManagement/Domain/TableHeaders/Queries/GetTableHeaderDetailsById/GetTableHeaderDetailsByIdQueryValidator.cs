using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Queries.GetTableHeaderDetailsById
{
    public class GetTableHeaderDetailsByIdQueryValidator : AbstractValidator<GetTableHeaderDetailsByIdQuery>
    {
        public GetTableHeaderDetailsByIdQueryValidator()
        {
            RuleFor(x => x.TableHeaderId)
                .NotEmpty();
        }
    }
}
