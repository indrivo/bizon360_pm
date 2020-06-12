using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Querries.GetCheckListForActivity
{
    public class GetCheckListForActivityQueryHandler : IRequestHandler<GetCheckListForActivityQuery, CheckListViewModel>
    {
        private readonly IGearContext _context;

        public GetCheckListForActivityQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<CheckListViewModel> Handle(GetCheckListForActivityQuery request, CancellationToken cancellationToken)
        {
            return new CheckListViewModel()
            {
                CheckList = (await _context.CheckItems.Where(x => x.ActivityId == request.ActivityId)
                    .Include(x => x.LoggedTime).ThenInclude(x => x.Tracker)
                    .Include(x => x.ApplicationUser)
                    .OrderBy(x => x.Order)
                    .ToListAsync(cancellationToken))
                    .Select(CheckItemDetailModel.Create).ToList()
            };
        }
    }
}
