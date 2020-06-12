using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserDetails
{
    public class GetApplicationUserDetailQuery : IRequest<ApplicationUserDetailModel>
    {
        public Guid Id { get; set; }
    }
}
