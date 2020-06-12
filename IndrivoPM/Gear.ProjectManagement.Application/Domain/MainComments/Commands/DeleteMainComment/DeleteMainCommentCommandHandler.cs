using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.MainComments.Commands.DeleteMainComment
{
    internal class DeleteMainCommentCommandHandler : IRequestHandler<DeleteMainCommentCommand>
    {
        private readonly IGearContext _context;

        public DeleteMainCommentCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteMainCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _context.MainComments.FindAsync(request.Id);

/*            if (comment == null)
            {
                throw new NotFoundException("");
            }*/

            _context.MainComments.Remove(comment);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
