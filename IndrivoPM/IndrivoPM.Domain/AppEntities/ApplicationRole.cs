using System;
using Microsoft.AspNetCore.Identity;

namespace Gear.Domain.AppEntities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string PlatformName { get; set; }

        public Platform Platform { get; set; }
    }
}
