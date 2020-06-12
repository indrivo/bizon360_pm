using MediatR;

namespace Gear.Identity.Manager.Domain.Platforms.Queeries.GetPlatformDetails
{
    public class GetPlatformQueryDetails : IRequest<PlatformDetailsModel>
    {
        public string PlatfromName { get; set; }
    }
}
