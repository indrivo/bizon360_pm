using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Queries.GetChangeRequestList
{
    public class GetChangeRequestListQueryHandler : IRequestHandler<GetChangeRequestListQuery, ChangeRequestListViewModel>
    {
        private readonly IGearContext _context;

        public GetChangeRequestListQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ChangeRequestListViewModel> Handle(GetChangeRequestListQuery request, CancellationToken cancellationToken)
        {
            if (request.Status != ChangeRequestStatus.None)
            {
                return new ChangeRequestListViewModel
                {
                    ChangeRequests = (await _context.ChangeRequests
                            .Include(cr => cr.Project)
                            .Where(cr => cr.ProjectId == request.ProjectId && cr.Status == request.Status)
                            .ToListAsync(cancellationToken))
                        .Select(cr => ChangeRequestLookupModel.Create(cr, _context.ApplicationUsers.Find(cr.CreatedBy)))
                        .ToList()
                };
            }

            return new ChangeRequestListViewModel
            {
                ChangeRequests = (await _context.ChangeRequests
                        .Include(cr => cr.Project)
                        .Where(cr => cr.ProjectId == request.ProjectId)
                        .ToListAsync(cancellationToken))
                    .Select(cr => ChangeRequestLookupModel.Create(cr, _context.ApplicationUsers.Find(cr.CreatedBy)))
                    .ToList()
            };
        }
    }
}
