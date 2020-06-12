using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.UpdateOrderActivities
{
    public class UpdateOrderActivitiesCommandValidator : AbstractValidator<UpdateOrderActivitiesCommand>
    {
        public UpdateOrderActivitiesCommandValidator()
        {
            RuleFor(x => x.ActivitiesIds).NotNull().NotEmpty();
        }
    }
}
