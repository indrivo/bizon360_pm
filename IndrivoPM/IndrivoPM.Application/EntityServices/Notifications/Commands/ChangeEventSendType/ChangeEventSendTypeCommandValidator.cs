using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Notifications.Commands.ChangeEventSendType
{
    public class ChangeEventSendTypeCommandValidator : AbstractValidator<ChangeEventSendTypeCommand>
    {
        public ChangeEventSendTypeCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
