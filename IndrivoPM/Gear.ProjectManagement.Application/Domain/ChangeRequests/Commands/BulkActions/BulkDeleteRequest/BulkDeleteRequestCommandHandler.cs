using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.BulkActions.BulkDeleteRequest
{
    public class BulkDeleteRequestCommandHandler : IRequestHandler<BulkDeleteRequestCommand, Unit>
    {
        private readonly IGearContext _context;

        public BulkDeleteRequestCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(BulkDeleteRequestCommand request, CancellationToken cancellationToken)
        {
            var requestsToDelete = new List<ChangeRequest>();

            foreach (var id in request.Requests)
            {
                var changeRequest = await _context.ChangeRequests.FindAsync(id);

                if (changeRequest == null)
                {
                    throw new NotFoundException(nameof(ChangeRequest), id);
                }

                requestsToDelete.Add(changeRequest);
            }

            if (!requestsToDelete.Any()) return Unit.Value;

            _context.ChangeRequests.RemoveRange(requestsToDelete);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
