using System.Collections.Generic;
using Gear.Identity.Permissions.Infrastructure.Resources.ViewModels;

namespace Gear.Identity.Manager.Domain.Platforms.Queeries.GetPlatformDetails
{
    public class PlatformDetailsModel
    {
        public string PlatformName { get; set; }

        public IList<RolesListDto> Roles { get; set; }

    }
}
