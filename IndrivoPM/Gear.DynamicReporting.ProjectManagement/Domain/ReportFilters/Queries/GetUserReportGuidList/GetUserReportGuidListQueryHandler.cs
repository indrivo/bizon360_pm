using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Queries.GetUserReportGuidList
{
    public class GetUserReportGuidListQueryHandler : IRequestHandler<GetUserReportGuidListQuery, UserReportGuidListModel>
    {
        private readonly IGearContext _context;

        public GetUserReportGuidListQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<UserReportGuidListModel> Handle(GetUserReportGuidListQuery request, CancellationToken cancellationToken)
        {
            var userReportGuidList = await _context.UserGuidFilters
                .Where(x => x.ReportId == request.ReportId 
                            && x.UserId == request.UserId 
                            && x.FilterType == request.FilterType)
                .ToListAsync(cancellationToken);

            if (userReportGuidList == null)
                return null;

            return new UserReportGuidListModel
            {
                GuidList = userReportGuidList.Select(x => x.EntityId).ToList()
            };
        }
    }
}
