using FluentValidation;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Queries.GetBusinessUnitDetails
{
    public class GetBusinessUnitDetailQueryValidator : AbstractValidator<GetBusinessUnitDetailQuery>
    {
        public GetBusinessUnitDetailQueryValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
