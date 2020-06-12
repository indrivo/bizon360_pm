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

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectMembersDto
{
    public class GetProjectMembersDtoQueryHandler : IRequestHandler<GetProjectMembersDtoQuery, List<SelectListItem>>
    {
        private readonly IGearContext _context;

        public GetProjectMembersDtoQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<List<SelectListItem>> Handle(GetProjectMembersDtoQuery request, CancellationToken cancellationToken)
        {
            var users = from pm in _context.ProjectMembers
                join u in _context.ApplicationUsers on pm.UserId equals u.Id
                where pm.ProjectId == request.Id
                select new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = $"{u.FirstName} {u.LastName}"
                };

            return await users.ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
