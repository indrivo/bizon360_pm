using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.BulkActions.BulkRefuseRequest
{
    public class BulkRefuseRequestCommandHandler : IRequestHandler<BulkRefuseRequestCommand, Unit>
    {
        private readonly IGearContext _context;

        public BulkRefuseRequestCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(BulkRefuseRequestCommand request, CancellationToken cancellationToken)
        {
            var requestsToRefuse = new List<ChangeRequest>();

            foreach (var id in request.Requests)
            {
                var changeRequest = await _context.ChangeRequests.FindAsync(id);

                if (changeRequest == null)
                {
                    throw new NotFoundException(nameof(ChangeRequest), id);
                }

                changeRequest.Status = ChangeRequestStatus.Refused;
                requestsToRefuse.Add(changeRequest);
            }

            if (!requestsToRefuse.Any()) return Unit.Value;

            _context.ChangeRequests.UpdateRange(requestsToRefuse);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
