using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectsDto
{
    public class GetProjectsDtoQuery: IRequest<List<SelectListItem>>
    {
        public bool GetAllProjects { get; set; }
    }
}
