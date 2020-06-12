using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.DeleteApplicationUsers
{
    public class DeleteApplicationUsersCommandHandler : IRequestHandler<DeleteApplicationUsersCommand, Unit>
    {
        private readonly IGearContext _context;

        public DeleteApplicationUsersCommandHandler(IGearContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(DeleteApplicationUsersCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var activitiesToDelete = await _context.ApplicationUsers.Where(item => request.UsersById.Contains(item.Id))
                    .ToListAsync(cancellationToken: cancellationToken);

                if (activitiesToDelete.Any())
                {
                    _context.ApplicationUsers.RemoveRange(activitiesToDelete);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"SaveChanges threw {e.GetType().Name}: {(e is DbUpdateException ? e.InnerException?.Message : e.Message)}");
            }


            return Unit.Value;
        }
    }
}
