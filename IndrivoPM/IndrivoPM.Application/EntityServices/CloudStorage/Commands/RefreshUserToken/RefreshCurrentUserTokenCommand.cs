using Gear.CloudStorage.Abstractions.Domain;
using MediatR;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Commands.RefreshUserToken
{
    /// <summary>
    /// Refresh currently logged user's token
    /// </summary>
    public class RefreshCurrentUserTokenCommand : IRequest
    {
        public ExternalProviders ExternalProvider { get; set; } = ExternalProviders.OneDrive;
    }
}
