using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.CreateCheckItem
{
    public class CreateCheckItemCommandHandler : IRequestHandler<CreateCheckItemCommand>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public CreateCheckItemCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(CreateCheckItemCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var checkItemOrder = (await _context.CheckItems.AnyAsync(x => x.ActivityId == request.ActivityId, cancellationToken) 
                ? (await _context.CheckItems
                    .Where(x => x.ActivityId == request.ActivityId)
                    .MaxAsync(x => x.Order, cancellationToken)) 
                : 0) + 1;

            var entity = new CheckItem
            {
                ActivityId = request.ActivityId,
                Content = request.Content,
                Order = checkItemOrder
            };

            entity.CreateEnd(Guid.NewGuid(), "No name provided",
                Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value));

            entity.ApplicationUserId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);

            await _context.CheckItems.AddAsync(entity, cancellationToken);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Unit.Value;
        }
    }
}
