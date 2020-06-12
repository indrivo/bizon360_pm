using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Notifications.Querries.GetNotificationProfile
{
    public class GetNotificationProfileQueryDetailValidator : AbstractValidator<GetNotificationProfileDetailQuery>
    {
        public GetNotificationProfileQueryDetailValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty();
        }
    }
}
