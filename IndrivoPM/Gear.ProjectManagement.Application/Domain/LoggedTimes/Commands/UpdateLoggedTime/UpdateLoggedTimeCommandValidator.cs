using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Commands.UpdateLoggedTime
{
    public class UpdateLoggedTimeCommandValidator : AbstractValidator<UpdateLoggedTimeCommand>
    {
        public UpdateLoggedTimeCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Time)
                .NotEmpty().WithMessage("You didn't set your time")
                .InclusiveBetween(0.1f, 12).WithMessage("You can log between 0.1 - 12 hours");

            RuleFor(x => x.ActivityId)
                .NotEmpty().WithMessage("You must choose an activity");

            RuleFor(x => x.TrackerId)
                .NotEmpty().WithMessage("You must choose a tracker type");
        }
    }
}
