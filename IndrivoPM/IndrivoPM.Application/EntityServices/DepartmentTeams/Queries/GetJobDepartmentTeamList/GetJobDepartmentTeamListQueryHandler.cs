using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gear.Domain.HrmEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.EntityServices.JobPositions.Queries.GetJobPositionList;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetJobDepartmentTeamList
{
    public class GetJobDepartmentTeamListQueryHandler : IRequestHandler<GetJobDepartmentTeamListQuery, JobPositionListViewModel>
    {
        private readonly IGearContext _context;
        private readonly IMapper _mapper;

        public GetJobDepartmentTeamListQueryHandler(IGearContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<JobPositionListViewModel> Handle(GetJobDepartmentTeamListQuery request, CancellationToken cancellationToken)
        {
            List<JobDepartmentTeam> jobIds = new List<JobDepartmentTeam>();

            foreach (var item in request.TeamIds)
            {
                jobIds.AddRange(_context.JobDepartmentTeams.Where(x => x.DepartmentTeamId == item).ToList());
            }

            List<JobPosition> jobs = new List<JobPosition>();

            foreach (var item in jobIds)
            {
                jobs.Add(_context.JobPositions.First(x => x.Id == item.JobPositionId));
            }

            return Task.FromResult(new JobPositionListViewModel()
            {
                JobPositions = jobs.OrderBy(x => x.RowOrder).Select(x => JobPositionLookupModel.Create(x)).ToList()
            });
        }
    }
}
