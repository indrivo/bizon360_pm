using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Notifications.Commands.UpdateNotificationProfile
{
    public class UpdateNotificationProfileCommandValidator : AbstractValidator<UpdateNotificationProfileCommand>
    {
        public UpdateNotificationProfileCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.EventList)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.UserList)
                .NotNull()
                .NotEmpty();
        }
    }
}
