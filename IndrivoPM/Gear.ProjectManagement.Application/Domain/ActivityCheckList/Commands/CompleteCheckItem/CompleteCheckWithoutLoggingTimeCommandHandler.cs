using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.CompleteCheckItem
{
    public class CompleteCheckWithoutLoggingTimeCommandHandler : IRequestHandler<CompleteCheckWithoutLoggingTimeCommand>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public CompleteCheckWithoutLoggingTimeCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(CompleteCheckWithoutLoggingTimeCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var entity = await _context.CheckItems.FindAsync(request.Id);

            entity.IsCompleted = true;
            entity.ApplicationUserId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            

            _context.CheckItems.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
