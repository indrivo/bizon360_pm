using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.StringExtensions;
using Gear.Domain.Infrastructure;
using Gear.ProjectManagement.Manager.Domain.MainComments.Commands;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.SubComments.Commands.UpdateSubComment
{
    internal class UpdateSubCommentCommandHandler : IRequestHandler<UpdateSubCommentCommand>
    {
        private readonly IGearContext _context;
        private readonly IMediator _mediator;


        public UpdateSubCommentCommandHandler(IGearContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateSubCommentCommand request, CancellationToken cancellationToken)
        {
            var subComment = await _context.SubComments.FindAsync(request.Id);

            subComment.UpdateComment(subComment, request.Message);

            _context.SubComments.Update(subComment);

            await _context.SaveChangesAsync(cancellationToken);

            // ------------------------------
            // Send email to mentioned users
            var emails = request.Message.FindEmails();

            if (emails.Count == 0) return Unit.Value;

            var mainComment = await _context.MainComments.FindAsync(subComment.MainCommentId);

            var notification = new SendCommentEmail()
            {
                PrimaryEntityId = mainComment.RecordId,
                UserName = subComment.AuthorName,
                Recipients = emails,
                AuthorName = subComment.AuthorName,
                Message = subComment.Message
            };

            await _mediator.Publish(notification);

            return Unit.Value;
        }
    }
}
