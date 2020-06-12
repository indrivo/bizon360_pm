using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetUserLoggedTimeByPeriod
{
    public class GetUserLoggedTimeByPeriodQueryValidator : AbstractValidator<GetUserLoggedTimeByPeriodQuery>
    {
        public GetUserLoggedTimeByPeriodQueryValidator()
        {
            RuleFor(a => a.UserId)
                .NotEmpty();

            RuleFor(a => a.StartDate)
                .LessThanOrEqualTo(a => a.EndDate).WithMessage("The starting date can't be greater than the ending one.");
        }
    }
}
