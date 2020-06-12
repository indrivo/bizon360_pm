using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.AppEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.BulkActions.BulkMoveCandidateToEmployee
{
    public class BulkMoveCandidateToEmployeeCommandHandler : IRequestHandler<BulkMoveCandidateToEmployeeCommand>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public BulkMoveCandidateToEmployeeCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(BulkMoveCandidateToEmployeeCommand request, CancellationToken cancellationToken)
        {
            foreach (var id in request.CandidatesId)
            {
                var candidate = await _context.Candidates.FindAsync(id);
                var operation = candidate.ChangeStatus(Domain.HrmEntities.Enums.CandidateStatus.Won);

                _context.Candidates.Update(candidate);

                var employee = new ApplicationUser
                {
                    FirstName = candidate.Name.FirstName,
                    LastName = candidate.Name.LastName,
                    Email = candidate.ContactInfo.Email,
                    JobPosition = candidate.JobPosition
                };

                await _context.ApplicationUsers.AddAsync(employee);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
