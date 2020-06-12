using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Querries.GetActivityListsDto
{
    public class GetActivityListsDtoQueryHandler : IRequestHandler<GetActivityListsDtoQuery, List<SelectListItem>>
    {
        private readonly IGearContext _context;

        public GetActivityListsDtoQueryHandler(IGearContext context)
        {
            _context = context;
        }
        public async Task<List<SelectListItem>> Handle(GetActivityListsDtoQuery request, CancellationToken cancellationToken)
        {
            var activityLists = _context.ActivityLists.Where(al => al.ProjectId == request.ProjectId)
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem {Value = x.Id.ToString(), Text = x.Name});


            return  await activityLists.ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
