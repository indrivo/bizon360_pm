using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.GetUserLoggedTime
{
    public class GetUserLoggedTimeQueryValidator : AbstractValidator<GetUserLoggedTimeQuery>
    {
        public GetUserLoggedTimeQueryValidator()
        {
            RuleFor(a => a.EmployeeId)
                .NotEmpty();

            RuleFor(a => a.StartDate)
                .LessThanOrEqualTo(a => a.DueDate).WithMessage("The starting date can't be greater than the due one.");
        }
    }
}
