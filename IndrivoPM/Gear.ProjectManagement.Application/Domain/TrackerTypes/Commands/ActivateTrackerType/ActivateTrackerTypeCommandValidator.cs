using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.ActivateTrackerType
{
    public class ActivateTrackerTypeCommandValidator : AbstractValidator<ActivateTrackerTypeCommand>
    {
        public ActivateTrackerTypeCommandValidator()
        {
            RuleFor(x => x.Ids).NotNull().NotEmpty();
        }
    }
}
