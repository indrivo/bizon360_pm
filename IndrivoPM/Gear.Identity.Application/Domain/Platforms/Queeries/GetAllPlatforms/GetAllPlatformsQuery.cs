using System.Collections.Generic;
using MediatR;

namespace Gear.Identity.Manager.Domain.Platforms.Queeries.GetAllPlatforms
{
    public class GetAllPlatformsQuery : IRequest<List<string>>
    {
    }
}
