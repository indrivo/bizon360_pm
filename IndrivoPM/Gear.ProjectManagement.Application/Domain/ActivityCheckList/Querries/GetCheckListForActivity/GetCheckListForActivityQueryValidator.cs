using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Querries.GetCheckListForActivity
{
    public class GetCheckListForActivityQueryValidator : AbstractValidator<GetCheckListForActivityQuery>
    {
        public GetCheckListForActivityQueryValidator()
        {
            RuleFor(x => x.ActivityId)
                .NotNull()
                .NotEmpty();
        }
    }
}
