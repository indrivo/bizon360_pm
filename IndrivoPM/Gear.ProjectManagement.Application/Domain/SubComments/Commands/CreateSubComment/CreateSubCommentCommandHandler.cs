using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.StringExtensions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using Gear.ProjectManagement.Manager.Domain.MainComments.Commands;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.SubComments.Commands.CreateSubComment
{
    internal class CreateSubCommentCommandHandler : IRequestHandler<CreateSubCommentCommand>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMediator _mediator;

        public CreateSubCommentCommandHandler(IGearContext context, IUserAccessor userAccessor, IMediator mediator)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mediator = mediator;
        }
        
        public async Task<Unit> Handle(CreateSubCommentCommand request, CancellationToken cancellationToken)
        {
            var userClaims = _userAccessor.GetUser();

            var userId = Guid.Parse(userClaims.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = await _context.ApplicationUsers.FindAsync(userId);

            var subComment = new SubComment(request.MainCommentId, user, request.Message);

            await _context.SubComments.AddAsync(subComment, cancellationToken);

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
                Message = subComment.Message,
                
            };

            await _mediator.Publish(notification);

            return Unit.Value;
        }
    }
}
