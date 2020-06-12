using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.UpdateCandidateStatus
{
    public class UpdateCandidateStatusCommandHandler : IRequestHandler<UpdateCandidateStatusCommand, Unit>
    {
        private readonly IGearContext _context;
        private readonly IMediator _mediator;
        private readonly IUserAccessor _userAccessor;

        public UpdateCandidateStatusCommandHandler(IGearContext context, IMediator mediator, IUserAccessor userAccessor)
        {
            _context = context;
            _mediator = mediator;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(UpdateCandidateStatusCommand request, CancellationToken cancellationToken)
        {
            var candidate = await _context.Candidates.FindAsync(request.Id);
            candidate.ChangeStatus(request.CandidateStatus);

            _context.Candidates.Update(candidate);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
