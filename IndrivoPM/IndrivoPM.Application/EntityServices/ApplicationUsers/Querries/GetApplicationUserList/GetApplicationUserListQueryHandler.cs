using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserList
{
    public class GetApplicationUserListQueryHandler : IRequestHandler<GetApplicationUserListQuery, ApplicationUserListViewModel>
    {
        private readonly IGearContext _context;
        private readonly IMapper _mapper;

        public GetApplicationUserListQueryHandler(IGearContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<ApplicationUserListViewModel> Handle(GetApplicationUserListQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new ApplicationUserListViewModel
            {
                ApplicationUsers = _context.ApplicationUsers.OrderBy(x => x.UserName)
                    .Include(x => x.JobPosition)
                    .Select(x => ApplicationUserLookupModel.Create(x)).ToList()
            });
        }
    }
}
