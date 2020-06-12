using MediatR;

namespace Gear.Identity.Manager.Domain.Roles.Queries.GetRoleDetails
{
    public class GetRoleDetailQuery : IRequest<RoleDetailModel>
    {
        public string RoleName { get; set; }
    }
}
