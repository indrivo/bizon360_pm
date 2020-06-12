using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.CloudStorage.Abstractions.Domain;
using Gear.CloudStorage.Abstractions.Infrastructure;
using Gear.CloudStorage.Abstractions.Service.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Gear.CloudStorage.Abstractions.Service
{
    public class BaseCloudUserDataService : IUserTokenDataService
    {
        private readonly ICloudStorageDb _applicationContext;

        public BaseCloudUserDataService(ICloudStorageDb applicationContext)
        {
            _applicationContext = applicationContext;
        }

        /// <summary>
        /// Set user token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <param name="userId"></param>
        /// <param name="provider"></param>
        /// <param name="accessToken"></param>
        public virtual async Task SetUpUserToken(string accessToken, string refreshToken, Guid userId, ExternalProviders provider)
        {
            var currentToken = await _applicationContext.AspNetUserTokens.FirstOrDefaultAsync(x =>
                x.UserId == userId && x.LoginProvider == provider.ToString());

            if (currentToken == null)
            {
                _applicationContext.AspNetUserTokens.Add(new IdentityUserToken<Guid>()
                {
                    UserId = userId,
                    Value = "{" + $"\"access_token\":\"{accessToken}\",\"refresh_token\":\"{refreshToken}\"" +
                            "}",
                    LoginProvider = provider.ToString(),
                    Name = provider + ":" + userId
                });
            }
            else
            {
                currentToken.Value = "{" + $"\"access_token\":\"{accessToken}\",\"refresh_token\":\"{refreshToken}\"" +
                                     "}";
                _applicationContext.AspNetUserTokens.Update(currentToken);
            }

            await _applicationContext.SaveChangesAsync(new CancellationToken(false));
        }

        /// <summary>
        /// Delete user token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="provider"></param>
        public virtual async Task DeleteUserToken(Guid userId, ExternalProviders provider)
        {
            _applicationContext.AspNetUserTokens.Remove
                (await _applicationContext.AspNetUserTokens.FirstOrDefaultAsync(x => x.UserId == userId && x.LoginProvider == provider.ToString()) 
                 ?? throw new InvalidOperationException());
            await _applicationContext.SaveChangesAsync(CancellationToken.None);
        }

        /// <summary>
        /// Get user Token for specified provider
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public virtual async Task<string> GetUserAccessToken(Guid userId, ExternalProviders provider)
        {
                return JObject.Parse((await _applicationContext.AspNetUserTokens.FirstOrDefaultAsync(x =>
                        x.UserId == userId && x.LoginProvider == provider.ToString()))
                    ?.Value).GetValue("access_token").Value<string>();
        }
        public virtual async Task<bool> CheckUserToken(Guid userId)
        {
            var exists = _applicationContext.AspNetUserTokens.Any(x => x.UserId == userId);

            string userToken = exists ? JObject.Parse((await _applicationContext.AspNetUserTokens.FirstOrDefaultAsync(x => x.UserId == userId))?.Value)?.GetValue("access_token")?.Value<string>() : null;

            return !string.IsNullOrEmpty(userToken);
        }

        /// <summary>
        /// Get user refresh Token for specified provider
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public virtual async Task<string> GetUserRefreshToken(Guid userId, ExternalProviders provider)
        {
            return JObject.Parse((await _applicationContext.AspNetUserTokens.FirstOrDefaultAsync(x =>
                x.UserId == userId && x.LoginProvider == provider.ToString()))
                ?.Value).GetValue("refresh_token").Value<string>();
        }
    }
}
