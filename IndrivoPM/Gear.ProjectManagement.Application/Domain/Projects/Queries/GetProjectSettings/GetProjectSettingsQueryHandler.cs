using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Settings;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectSettings
{
    public class GetProjectSettingsQueryHandler : IRequestHandler<GetProjectSettingsQuery,ProjectSettingsModel>
    {
        private readonly IGearContext _context;
        private readonly IMapper _mapper;

        public GetProjectSettingsQueryHandler(IGearContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectSettingsModel> Handle(GetProjectSettingsQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.ProjectSettings.FirstAsync(x => x.ProjectId == request.ProjectId, cancellationToken: cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProjectSettings), request.ProjectId);
            }
            return _mapper.Map<ProjectSettingsModel>(entity);
        }
    }
}
