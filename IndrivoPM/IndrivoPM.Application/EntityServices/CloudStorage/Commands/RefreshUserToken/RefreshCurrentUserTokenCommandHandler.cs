using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.CloudStorage.Abstractions.Domain;
using Gear.CloudStorage.Abstractions.Service.Abstractions;
using Gear.Common.Extensions.UserInjection;
using MediatR;
using Microsoft.Extensions.Options;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Commands.RefreshUserToken
{
    public class RefreshCurrentUserTokenCommandHandler : IRequestHandler<RefreshCurrentUserTokenCommand>
    {
        private readonly ICloudUserAuthorizationService _authorizationService;

        private readonly IUserTokenDataService _tokenDataService;

        private readonly IUserAccessor _userAccessor;

        /// <summary>
        /// Configurations for the One Drive Settings
        /// </summary>
        private readonly IOptionsMonitor<CloudServiceSettings> _config;

        public RefreshCurrentUserTokenCommandHandler(ICloudUserAuthorizationService authorizationService,
            IUserAccessor userAccessor, IOptionsMonitor<CloudServiceSettings> config, IUserTokenDataService tokenDataService)
        {
            _authorizationService = authorizationService;
            _userAccessor = userAccessor;
            _config = config;
            _tokenDataService = tokenDataService;
        }

        public async Task<Unit> Handle(RefreshCurrentUserTokenCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value);
            var accessToken = await _tokenDataService.GetUserAccessToken(userId, request.ExternalProvider);
            var userRefreshToken = await _tokenDataService.GetUserRefreshToken(userId, request.ExternalProvider);
            await _authorizationService.VerifyUser(accessToken, userRefreshToken
                 , userId, request.ExternalProvider, _config.CurrentValue.ClientId,
                _config.CurrentValue.ReturnUrl, _config.CurrentValue.ClientSecret);

            return Unit.Value;
        }
    }
}
