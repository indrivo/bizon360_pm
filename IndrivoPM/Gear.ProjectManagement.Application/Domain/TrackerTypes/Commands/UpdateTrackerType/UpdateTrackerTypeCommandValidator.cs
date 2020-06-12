using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.UpdateTrackerType
{
    public class UpdateTrackerTypeCommandValidator:AbstractValidator<UpdateTrackerTypeCommand>
    {
        public UpdateTrackerTypeCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Name).MaximumLength(256)
                .NotEmpty();
        }
    }
}
