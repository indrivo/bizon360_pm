using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Notifications.Commands.CreateNotificationProfile
{
    public class CreateNotificationProfileCommandValidator : AbstractValidator<CreateNotificationProfileCommand>
    {
        public CreateNotificationProfileCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.UserList)
                .NotNull()
                .NotEmpty();
        }
    }
}
