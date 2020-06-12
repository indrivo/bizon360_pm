using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.CloudStorage.Abstractions.Domain;
using Gear.CloudStorage.Abstractions.Service.Abstractions;
using Gear.Common.Extensions.HttpClient;
using Gear.Common.Extensions.UserInjection;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Commands.RegisterUserToken
{
    public class RegisterUserTokenCommandHandler : IRequestHandler<RegisterUserTokenCommand>
    {
        private readonly IUserTokenDataService _userTokenDataService;

        private readonly IUserAccessor _userAccessor;

        private readonly IHttpClientAccessor _accessor;

        /// <summary>
        /// Configurations for the One Drive Settings
        /// </summary>
        private readonly IOptionsMonitor<CloudServiceSettings> _config;

        public RegisterUserTokenCommandHandler(IStorageBaseService service, 
            IUserTokenDataService userTokenDataService, IOptionsMonitor<CloudServiceSettings> config, 
            IUserAccessor userAccessor, IHttpClientAccessor accessor)
        {
            _userTokenDataService = userTokenDataService;
            _config = config;
            _userAccessor = userAccessor;
            _accessor = accessor;
        }


        public async Task<Unit> Handle(RegisterUserTokenCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var dict = new Dictionary<string, string>
            {
                {"client_id", _config.CurrentValue.ClientId},
                {"redirect_uri", _config.CurrentValue.ReturnUrl},
                {"client_secret", _config.CurrentValue.ClientSecret},
                {"code", request.OAuthCode},
                {"grant_type", "authorization_code"}
            };

            _accessor.Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var postAction = await _accessor.Client.PostAsync("https://login.microsoftonline.com/common/oauth2/v2.0/token", new FormUrlEncodedContent(dict), cancellationToken);
            var result = JsonConvert.DeserializeObject<CloudLoginModel>(await postAction.Content.ReadAsStringAsync());
            await _userTokenDataService.SetUpUserToken(result.AccessToken, result.RefreshToken, Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value),
                ExternalProviders.OneDrive);

            return Unit.Value;
        }
    }
}
