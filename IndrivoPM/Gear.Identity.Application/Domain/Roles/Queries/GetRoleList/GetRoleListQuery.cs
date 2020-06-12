using System.Collections.Generic;
using Gear.Identity.Permissions.Infrastructure.Resources.ViewModels;
using MediatR;

namespace Gear.Identity.Manager.Domain.Roles.Queries.GetRoleList
{
    public class GetRoleListQuery : IRequest<List<RolesListDto>>
    {
    }
}
