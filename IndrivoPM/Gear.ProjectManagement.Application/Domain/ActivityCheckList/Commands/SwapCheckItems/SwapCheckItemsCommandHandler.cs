using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.SwapCheckItems
{
    public class SwapCheckItemsCommandHandler : IRequestHandler<SwapCheckItemsCommand>
    {
        private readonly IGearContext _context;

        public SwapCheckItemsCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SwapCheckItemsCommand request, CancellationToken cancellationToken)
        {
            var checkItems = await _context.CheckItems
                .Where(x => request.Ids.Contains(x.Id))
                .ToListAsync(cancellationToken);

            var index = 0;
            foreach (var id in request.Ids)
            {
                var checkItem = checkItems.Find(x => x.Id == id);
                checkItem.Order = ++index;
            }
            _context.CheckItems.UpdateRange(checkItems);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
