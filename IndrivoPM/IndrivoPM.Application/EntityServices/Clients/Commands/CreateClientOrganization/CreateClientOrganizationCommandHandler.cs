using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.CrmEntities.Primary;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Clients.Commands.CreateClientOrganization
{
    public class CreateClientOrganizationCommandHandler : IRequestHandler<CreateClientOrganizationCommand>
    {
        private readonly IGearContext _context;

        private readonly IUserAccessor _userAccessor;

        private readonly IMediator _mediator;

        public CreateClientOrganizationCommandHandler(IGearContext context, IUserAccessor userAccessor, IMediator mediator)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CreateClientOrganizationCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser(); 

            //Create entity
            var entity = new ClientOrganization(request.Id, request.Name, request.Address, 
                request.ContactInfo, Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value));

          //  await _context.ClientOrganizations.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            //Send notification
            await _mediator.Publish(new ClientCompanyCreated()
            {
                PrimaryEntityId = entity.Id,
                GroupEntityId = entity.Id,
                GroupEntityName = entity.Name,
                PrimaryEntityName = entity.Name,
                UserName = user.Identity.Name
            }, cancellationToken);

            return Unit.Value;
        }
    }
}
