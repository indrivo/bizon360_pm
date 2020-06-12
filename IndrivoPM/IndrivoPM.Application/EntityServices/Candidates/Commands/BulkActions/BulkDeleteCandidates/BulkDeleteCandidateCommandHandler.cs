using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.HrmEntities.Recruitment;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.BulkActions.BulkDeleteCandidates
{
    public class BulkDeleteCandidateCommandHandler : IRequestHandler<BulkDeleteCandidateCommand, Unit>
    {
        private readonly IGearContext _context;

        public BulkDeleteCandidateCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(BulkDeleteCandidateCommand request, CancellationToken cancellationToken)
        {
            var candidatesLost = new List<Candidate>();

            foreach (var id in request.Candidates)
            {
                var candidate = await _context.Candidates.FindAsync(id);

                if (candidate == null)
                {
                    throw new NotFoundException(nameof(Candidate), id);
                }

                candidate.ChangeStatus(Domain.HrmEntities.Enums.CandidateStatus.Lost);

                candidatesLost.Add(candidate);
            }

            if (candidatesLost.Any())
            {
                _context.Candidates.UpdateRange(candidatesLost);

                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
