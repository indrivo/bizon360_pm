using System;
using Gear.Identity.Permissions.Domain.Resources;

namespace Gear.Identity.Permissions.Domain.Entities
{
    /// <summary>
    /// Db model for holding what modules/features the user can access
    /// </summary>
    public class ModulesForUser
    {
        public Guid UserId { get; set; }

        public PaidForModules AllowedPaidForModules { get; set; }

    }
}
