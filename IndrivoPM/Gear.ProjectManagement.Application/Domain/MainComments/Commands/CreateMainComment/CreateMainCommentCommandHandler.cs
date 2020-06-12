using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.StringExtensions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.MainComments.Commands.CreateMainComment
{
    internal class CreateMainCommentCommandHandler : IRequestHandler<CreateMainCommentCommand>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMediator _mediator;

        public CreateMainCommentCommandHandler(IUserAccessor userAccessor, IGearContext context, IMediator mediator)
        {
            _userAccessor = userAccessor;
            _context = context;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CreateMainCommentCommand request, CancellationToken cancellationToken)
        {
            var userClaims = _userAccessor.GetUser();

            var userId = Guid.Parse(userClaims.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = await _context.ApplicationUsers.FirstAsync(x => x.Id == userId, cancellationToken);
            
            var comment = new MainComment(request.RecordId, user, request.Message);

            _context.MainComments.Add(comment);

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
