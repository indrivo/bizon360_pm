using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Notifications.Commands.AssignUserNotificationProfile
{
    public class AssignUserNotificationProfileCommandValidator : AbstractValidator<AssignUserNotificationProfileCommand>
    {
        public AssignUserNotificationProfileCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
