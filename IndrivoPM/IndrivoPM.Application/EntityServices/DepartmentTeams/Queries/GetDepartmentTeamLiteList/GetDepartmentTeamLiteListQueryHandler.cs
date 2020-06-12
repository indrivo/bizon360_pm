using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gear.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Application.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamsList
{
    public class GetDepartmentTeamListQueryHandler : IRequestHandler<GetDepartmentTeamListQuery, DepartmentTeamListViewModel>
    {
        private readonly IGearContext _context;
        private readonly IMapper _mapper;

        public GetDepartmentTeamListQueryHandler(IGearContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<DepartmentTeamListViewModel> Handle(GetDepartmentTeamListQuery request, CancellationToken cancellationToken)
        {
            if (request.DepartmentId == Guid.Empty)
            {
                return new DepartmentTeamListViewModel()
                {
                    DepartmentTeams = (await _context.DepartmentTeams.OrderBy(x => x.Name)
                        .Include(x => x.Department)
                        .Include(x => x.JobDepartmentTeams)
                        .ThenInclude(x => x.JobPosition)
                        .Include(x => x.DepartmentTeamLead)
                        .Include(x => x.UserDepartmentTeams)
                        .ThenInclude(x => x.User).ToListAsync(cancellationToken)).Select(x => DepartmentTeamLookupModel.Create(x)).ToList()
                };
            }

            return new DepartmentTeamListViewModel
            {
                DepartmentTeams = (await _context.DepartmentTeams
                        .Include(x => x.Department)
                        .Include(x => x.JobDepartmentTeams)
                        .ThenInclude(x => x.JobPosition)
                        .Include(x => x.DepartmentTeamLead).Where(x => x.DepartmentId == request.DepartmentId)
                        .Include(x => x.UserDepartmentTeams)
                        .ThenInclude(x => x.User).ToListAsync(cancellationToken)).Select(x => DepartmentTeamLookupModel.Create(x)).ToList()
            };

        }
    }
}
