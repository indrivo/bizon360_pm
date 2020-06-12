using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Notifications.Querries.GetEvent
{
    public class GetEventDataQueryValidator : AbstractValidator<GetEventDataQuery>
    {
        public GetEventDataQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty();
        }
    }
}
