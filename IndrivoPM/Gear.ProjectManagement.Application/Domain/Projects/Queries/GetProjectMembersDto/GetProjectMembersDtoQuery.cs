using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectMembersDto
{
   public class GetProjectMembersDtoQuery : IRequest<List<SelectListItem>>
    {
        public Guid Id { get;set;}
    }
}
