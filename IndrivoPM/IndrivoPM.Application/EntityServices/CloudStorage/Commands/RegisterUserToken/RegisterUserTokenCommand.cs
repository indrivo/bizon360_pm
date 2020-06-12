using MediatR;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Commands.RegisterUserToken
{
    public class RegisterUserTokenCommand : IRequest
    {
        public string OAuthCode { get; set; }
    }
}
