using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.RenameCheckItem
{
    public class RenameCheckItemCommandHandler : IRequestHandler<RenameCheckItemCommand>
    {
        private readonly IGearContext _context;

        public RenameCheckItemCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RenameCheckItemCommand request, CancellationToken cancellationToken)
        {
            var checkItem = await _context.CheckItems.FindAsync(request.Id);
            if (checkItem == null)
                throw new NotFoundException(nameof(CheckItem), request.Id);

            checkItem.Content = request.Content;

            _context.CheckItems.Update(checkItem);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
