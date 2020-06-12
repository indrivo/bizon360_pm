using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Queries.GetMonthlyLogsByActivity
{
    public class GetMonthlyLogsByActivityQueryValidator : AbstractValidator<GetMonthlyLogsByActivityQuery>
    {
        public GetMonthlyLogsByActivityQueryValidator()
        {
            RuleFor(request => request.EmployeeId).NotNull().NotEmpty();
        }
    }
}
