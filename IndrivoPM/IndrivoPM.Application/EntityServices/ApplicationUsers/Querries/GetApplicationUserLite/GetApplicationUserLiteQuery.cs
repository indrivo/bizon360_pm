using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserLite
{
    public class GetApplicationUserLiteQuery : IRequest<ApplicationUserLiteModel>
    {
        public Guid Id { get; set; }
    }
}
