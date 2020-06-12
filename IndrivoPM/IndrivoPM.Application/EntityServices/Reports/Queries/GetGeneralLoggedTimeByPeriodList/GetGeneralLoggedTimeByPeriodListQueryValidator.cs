using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetGeneralLoggedTimeByPeriodList
{
    public class GetGeneralLoggedTimeByPeriodListQueryValidator : AbstractValidator<GetGeneralLoggedTimeByPeriodListQuery>
    {
        public GetGeneralLoggedTimeByPeriodListQueryValidator()
        {
            RuleFor(a => a.StartDate)
                .LessThanOrEqualTo(a => a.DueDate).WithMessage("The starting date can't be greater than the due one.");
        }
    }
}
