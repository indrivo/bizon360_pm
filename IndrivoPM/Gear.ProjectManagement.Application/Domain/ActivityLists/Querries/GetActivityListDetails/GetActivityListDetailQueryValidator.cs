using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Querries.GetActivityListDetails
{
    public class GetActivityListDetailQueryValidator : AbstractValidator<GetActivityListDetailQuery>
    {
        public GetActivityListDetailQueryValidator()
        {
            RuleFor(al => al.Id).NotEmpty();
        }
    }
}
