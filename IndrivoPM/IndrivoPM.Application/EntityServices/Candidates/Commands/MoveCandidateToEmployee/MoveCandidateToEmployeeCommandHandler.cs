using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.AppEntities;
using Gear.Domain.HrmEntities.Enums;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.MoveCandidateToEmployee
{
    public class MoveCandidateToEmployeeCommandHandler : IRequestHandler<MoveCandidateToEmployeeCommand>
    {
        private readonly IGearContext _context;

        public MoveCandidateToEmployeeCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(MoveCandidateToEmployeeCommand request, CancellationToken cancellationToken)
        {
            var candidate = await _context.Candidates.FindAsync(request.CandidateId);
            candidate.ChangeStatus(CandidateStatus.Won);

            _context.Candidates.Update(candidate);

            var employee = new ApplicationUser
            {
                FirstName = candidate.Name.FirstName,
                LastName = candidate.Name.LastName,
                Email = candidate.ContactInfo.Email,
                JobPosition = candidate.JobPosition
            };
            
            //TODO: Add password management here???
            await _context.ApplicationUsers.AddAsync(employee, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
