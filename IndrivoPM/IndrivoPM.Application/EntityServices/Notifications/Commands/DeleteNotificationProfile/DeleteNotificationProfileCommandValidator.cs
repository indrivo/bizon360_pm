using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Notifications.Commands.DeleteNotificationProfile
{
    public class DeleteNotificationProfileCommandValidator : AbstractValidator<DeleteNotificationProfileCommand>
    {
        public DeleteNotificationProfileCommandValidator()
        {
            RuleFor(x => x.Ids)
                .NotNull()
                .NotEmpty();
        }
    }
}
