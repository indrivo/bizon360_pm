using System.Security.Claims;

namespace Gear.Common.Extensions.UserInjection
{
    public interface IUserAccessor
    {
        ClaimsPrincipal GetUser();
    }
}
