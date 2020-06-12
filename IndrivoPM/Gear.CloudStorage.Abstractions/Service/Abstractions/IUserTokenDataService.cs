using System;
using System.Threading.Tasks;
using Gear.CloudStorage.Abstractions.Domain;

namespace Gear.CloudStorage.Abstractions.Service.Abstractions
{
    public interface IUserTokenDataService
    {
        Task DeleteUserToken(Guid userId, ExternalProviders provider);

        Task<string> GetUserAccessToken(Guid userId, ExternalProviders provider);

        Task<string> GetUserRefreshToken(Guid userId, ExternalProviders provider);

        Task SetUpUserToken(string accessToken, string refreshToken, Guid userId, ExternalProviders provider);
        Task<bool> CheckUserToken(Guid userId);
    }
}
