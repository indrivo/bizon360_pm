using FluentValidation;
using Gear.Domain.PmEntities.Enums;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityListFromParentEntity
{
    public class GetActivityListFromParentEntityQueryValidator : AbstractValidator<GetActivityListFromParentEntityQuery>
    {
        public GetActivityListFromParentEntityQueryValidator()
        {
            RuleFor(request => request.ParentEntityId).NotNull().NotEmpty();

            RuleFor(request => request.ParentType).NotEqual(ActivityParentType.None);
        }
    }
}
