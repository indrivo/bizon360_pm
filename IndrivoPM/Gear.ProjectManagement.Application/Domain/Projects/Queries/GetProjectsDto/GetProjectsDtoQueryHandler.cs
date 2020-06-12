using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectsDto
{
    public class GetProjectsDtoQueryHandler:IRequestHandler<GetProjectsDtoQuery, List<SelectListItem>>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;
        public GetProjectsDtoQueryHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }
        public async Task<List<SelectListItem>> Handle(GetProjectsDtoQuery request, CancellationToken cancellationToken)
        {
            var projects = request.GetAllProjects
                ? _context.Projects
                : _context.Projects.Where(p => p.ProjectMembers.Any(x =>
                    x.UserId == Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value)));

            return await projects.Select(x => new SelectListItem {Value = x.Id.ToString(), Text = x.Name})
                .ToListAsync(cancellationToken);
        }
    }
}
