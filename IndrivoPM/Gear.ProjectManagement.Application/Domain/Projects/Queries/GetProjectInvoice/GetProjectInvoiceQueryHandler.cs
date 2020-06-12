using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Settings;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectInvoice
{
    internal class GetProjectInvoiceQueryHandler : IRequestHandler<GetProjectInvoiceQuery, ProjectInvoiceModel>
    {
        private readonly IGearContext _context;
        private readonly IMapper _mapper;

        public GetProjectInvoiceQueryHandler(IGearContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectInvoiceModel> Handle(GetProjectInvoiceQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.ProjectInvoices
                .FirstAsync(x => x.ProjectId == request.ProjectId, cancellationToken: cancellationToken);

            if (entity == null) throw new NotFoundException(nameof(ProjectInvoice), request.ProjectId);


            return ProjectInvoiceModel.Create(entity);
        }
    }
}
