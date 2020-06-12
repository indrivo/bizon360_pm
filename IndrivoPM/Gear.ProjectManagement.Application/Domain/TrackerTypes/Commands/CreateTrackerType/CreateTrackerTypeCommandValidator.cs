using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.CreateTrackerType
{
    public class CreateTrackerTypeCommandValidator:AbstractValidator<CreateTrackerTypeCommand>
    {
        public CreateTrackerTypeCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .MaximumLength(256);
        }
    }
}
