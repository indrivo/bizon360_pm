using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.StringExtensions;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.MainComments.Commands.UpdateMainComment
{
    internal class UpdateMainCommentCommandHandler : IRequestHandler<UpdateMainCommentCommand>
    {
        private readonly IGearContext _context;
        private readonly IMediator _mediator;

        public UpdateMainCommentCommandHandler(IGearContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateMainCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _context.MainComments.FindAsync(request.Id);

            comment.UpdateComment(comment, request.Message);

            _context.MainComments.Update(comment);

            await _context.SaveChangesAsync(cancellationToken);

            // ----------------
            // Send email to mentioned users
            var emails = request.Message.FindEmails();

            if (emails.Count == 0) return Unit.Value;

            var notification = new SendCommentEmail()
            {
                PrimaryEntityId = comment.RecordId,
                UserName = comment.AuthorName,
                Recipients = emails,
                AuthorName = comment.AuthorName,
                Message = comment.Message
            };

            await _mediator.Publish(notification);

            return Unit.Value;
        }
    }
}
