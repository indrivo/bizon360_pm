using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Notifications.Commands.UpdateEvent
{
    public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
    {
        public UpdateEventCommandValidator()
        {
            RuleFor(x => x.Event.Id)
                .NotNull();

            RuleFor(x => x.Event.NotificationTypes)
                .NotEmpty();

            RuleFor(x => x.Event.PropagationTypes)
                .NotEmpty();
        }
    }
}
