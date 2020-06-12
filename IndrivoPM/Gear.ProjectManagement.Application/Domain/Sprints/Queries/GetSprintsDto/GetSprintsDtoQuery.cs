using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Queries.GetSprintsDto
{
    public class GetSprintsDtoQuery : IRequest<List<SelectListItem>>
    {
        public Guid ProjectId { get;set;}
    }
}
