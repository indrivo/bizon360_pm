using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetLoggedTimeByPeriodList
{
    public class GetLoggedTimeByPeriodListQueryValidator : AbstractValidator<GetLoggedTimeByPeriodListQuery>
    {
        public GetLoggedTimeByPeriodListQueryValidator()
        {
            RuleFor(a => a.UserIds)
                .NotEmpty();

            RuleFor(a => a.StartDate)
                .LessThanOrEqualTo(a => a.DueDate).WithMessage("The starting date can't be greater than the due one.");
        }
    }
}
