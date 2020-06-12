using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Gear.CloudStorage.Abstractions.Domain;
using Gear.CloudStorage.Abstractions.Service.Abstractions;
using Newtonsoft.Json;

namespace Gear.OneDriveCloudStorage.Service
{
    public class OneDriveUserAuthorizationService : ICloudUserAuthorizationService
    {
        private readonly IUserTokenDataService _dataService;

        public OneDriveUserAuthorizationService(IUserTokenDataService dataService)
        {
            _dataService = dataService;
        }

        /// <summary>
        /// Verify User Token Status
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <param name="userId"></param>
        /// <param name="provider"></param>
        private async Task<bool> IsTokenAlive(string accessToken, string refreshToken, Guid userId, ExternalProviders provider)
        {
            var request = new HttpClient();
            request.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            try
            {
                var response = await request.GetAsync("https://graph.microsoft.com/v1.0/drive/root/children/");
                return response.StatusCode != HttpStatusCode.Unauthorized;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// Refresh user token
        /// By providing the refresh token from database to the Graph API
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="provider"></param>
        /// <param name="clientId"></param>
        /// <param name="redirectUri"></param>
        /// <param name="clientSecret"></param>
        private async Task RefreshUserToken(Guid userId, ExternalProviders provider, string clientId,
            string redirectUri, string clientSecret)
        {
            var dict = new Dictionary<string, string>
            {
                {"client_id", clientId},
                {"redirect_uri", redirectUri},
                {"client_secret", clientSecret},
                {"refresh_token", await _dataService.GetUserRefreshToken(userId,provider)},
                {"grant_type", "refresh_token"}
            };

            var client = new HttpClient();
            dict.TryGetValue("refresh_token", out var refreshToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var postAction = await client.PostAsync("https://login.microsoftonline.com/common/oauth2/v2.0/token", 
                new FormUrlEncodedContent(dict));
            var result = JsonConvert.DeserializeObject<CloudLoginModel>(await postAction.Content.ReadAsStringAsync());
            await _dataService.SetUpUserToken(result.AccessToken, refreshToken, userId, provider);
        }


        /// <summary>
        /// Call this method to trigger the User Token Refresh Use Case
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <param name="userId"></param>
        /// <param name="provider"></param>
        /// <param name="clientId"></param>
        /// <param name="redirectUri"></param>
        /// <param name="clientSecret"></param>
        public async Task VerifyUser(string accessToken, string refreshToken, Guid userId, ExternalProviders provider, string clientId,
            string redirectUri, string clientSecret)
        {
            try
            {
                if (IsTokenAlive(accessToken, refreshToken, userId, provider).Result)
                    await _dataService.SetUpUserToken(accessToken, refreshToken, userId, provider);
                else
                    await RefreshUserToken(userId, provider, clientId,
                        redirectUri, clientSecret);
            }
            catch
            {
                //
            }
        }

    }
}
