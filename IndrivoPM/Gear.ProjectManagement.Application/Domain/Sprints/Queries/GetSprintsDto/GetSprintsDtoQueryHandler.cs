using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Queries.GetSprintsDto
{
    public class GetSprintListDtoQueryHandler : IRequestHandler<GetSprintsDtoQuery, List<SelectListItem>>
    {
        private readonly IGearContext _context;

        public GetSprintListDtoQueryHandler(IGearContext context)
        {
            _context = context;
        }
        public async Task<List<SelectListItem>> Handle(GetSprintsDtoQuery request, CancellationToken cancellationToken)
        {
            var sprints = _context.Sprints.Where(s => s.ProjectId == request.ProjectId)
                .Where(s => s.SprintStatus != SprintStatus.Completed)
                .OrderBy(s => s.EndDate).ThenBy(s => s.Name)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });

            return await sprints.ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
