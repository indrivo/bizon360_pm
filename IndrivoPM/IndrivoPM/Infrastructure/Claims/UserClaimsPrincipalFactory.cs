using System.Security.Claims;
using System.Threading.Tasks;
using Gear.Domain.AppEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Bizon360.Infrastructure.Claims
{
    public class UserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        public UserClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("userId", user.Id.ToString()));
            return identity;
        }
    }

}
