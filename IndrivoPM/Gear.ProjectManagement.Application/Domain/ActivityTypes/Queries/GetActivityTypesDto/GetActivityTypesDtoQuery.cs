using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Queries.GetActivityTypesDto
{
    public class GetActivityTypesDtoQuery: IRequest<List<SelectListItem>>
    {
    }
}
