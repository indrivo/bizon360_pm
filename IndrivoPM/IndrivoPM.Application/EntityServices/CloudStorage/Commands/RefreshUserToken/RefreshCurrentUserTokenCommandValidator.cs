using FluentValidation;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Commands.RefreshUserToken
{
    public class RefreshCurrentUserTokenCommandValidator : AbstractValidator<RefreshCurrentUserTokenCommand>
    {
        public RefreshCurrentUserTokenCommandValidator()
        {
            RuleFor(x => x.ExternalProvider)
                .NotNull();
        }
    }
}
