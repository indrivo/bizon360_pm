using FluentValidation;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Commands.RegisterUserToken
{
    public class RegisterUserTokenCommandValidator : AbstractValidator<RegisterUserTokenCommand>
    {
        public RegisterUserTokenCommandValidator()
        {
            RuleFor(x => x.OAuthCode)
                .NotNull()
                .NotEmpty();
        }
    }
}
