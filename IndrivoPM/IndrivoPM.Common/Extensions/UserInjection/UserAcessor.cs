using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Gear.Common.Extensions.UserInjection
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _accessor;

        public UserAccessor(IHttpContextAccessor accessor)
        {
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
        }

        public ClaimsPrincipal GetUser() => _accessor.HttpContext.User;

    }
}
