using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Commands.CreateLoggedTime
{
    public class CreateLoggedTimeCommandValidator: AbstractValidator<CreateLoggedTimeCommand>
    {
        public CreateLoggedTimeCommandValidator()
        {
            RuleFor(x => x.Time).NotEmpty()
                .InclusiveBetween(0.1f, 12);

            RuleFor(x => x.ActivityId).NotEmpty();

            RuleFor(x => x.TrackerId).NotEmpty();
        }
    }
}
