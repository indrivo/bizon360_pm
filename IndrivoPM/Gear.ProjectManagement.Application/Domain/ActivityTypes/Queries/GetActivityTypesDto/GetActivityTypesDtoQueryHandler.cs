using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Queries.GetActivityTypesDto
{
    public class GetActivityTypesDtoQueryHandler : IRequestHandler<GetActivityTypesDtoQuery, List<SelectListItem>>
    {
        private readonly IGearContext _context;

        public GetActivityTypesDtoQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<List<SelectListItem>> Handle(GetActivityTypesDtoQuery request, CancellationToken cancellationToken)
        {
            var activityTypes = _context.ActivityTypes.OrderBy(x => x.RowOrder)
                .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });

            return await activityTypes.ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
