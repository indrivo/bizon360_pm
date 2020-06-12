using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.SubComments.Commands.DeleteSubComment
{
    internal class DeleteSubCommentCommandHandler : IRequestHandler<DeleteSubCommentCommand>
    {
        private readonly IGearContext _context;

        public DeleteSubCommentCommandHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteSubCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _context.SubComments.FindAsync(request.Id);

/*            if (comment == null)
            {
                throw new NotFoundException("");
            }*/

            _context.SubComments.Remove(comment);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
