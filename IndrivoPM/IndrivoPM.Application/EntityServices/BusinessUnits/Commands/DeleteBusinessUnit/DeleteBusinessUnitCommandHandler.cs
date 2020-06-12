using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.HrmEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.BusinessUnits.Commands.DeleteBusinessUnit
{
    public class DeleteBusinessUnitCommandHandler : IRequestHandler<DeleteBusinessUnitCommand>
    {
        private readonly IGearContext _context;

        private readonly IUserAccessor _userAccessor;

        public DeleteBusinessUnitCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(DeleteBusinessUnitCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.BusinessUnits.FindAsync(request.Id);

            var userId = Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value);

            if (entity == null)
            {
                throw new NotFoundException(nameof(BusinessUnit), request.Id);
            }

            if (!entity.IsDeletable)
            {
                throw new DeleteFailureException(nameof(BusinessUnit), request.Id, DeleteFailureMessages.EntityNotDeletable.ToString());

            }

            var hasDepartments = _context.Departments.Where(x => x.BusinessUnitId == request.Id);

            if (hasDepartments.Any())
            {
                foreach (var item in hasDepartments)
                {
                    item.BusinessUnitId = _context.BusinessUnits.First(x => !x.IsDeletable).Id;
                    item.Deactivate();
                    _context.Departments.Update(item);
                }
            }

            //TODO:Remake: don't remove the entity just deactivate it
            _context.BusinessUnits.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
