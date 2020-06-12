using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.UpdateOrderTrackerType
{
    public class UpdateOrderTrackerTypeCommandValidator : AbstractValidator<UpdateOrderTrackerTypeCommand>
    {
        public UpdateOrderTrackerTypeCommandValidator()
        {
            RuleFor(x => x.TrackerTypeIds).NotNull();
        }
    }
}
