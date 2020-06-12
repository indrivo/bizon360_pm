using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Querries.GetActivityListsDto
{
    public class GetActivityListsDtoQuery : IRequest<List<SelectListItem>>
    {
        public Guid ProjectId { get; set; }
    }
}
