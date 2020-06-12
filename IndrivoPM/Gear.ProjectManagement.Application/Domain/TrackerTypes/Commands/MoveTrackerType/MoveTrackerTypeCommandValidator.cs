using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.MoveTrackerType
{
    public class MoveTrackerTypeCommandValidator : AbstractValidator<MoveTrackerTypeCommand>
    {
        public MoveTrackerTypeCommandValidator()
        {
            RuleFor(x => x.ActivityTypeId).NotNull();
        }
    }
}
