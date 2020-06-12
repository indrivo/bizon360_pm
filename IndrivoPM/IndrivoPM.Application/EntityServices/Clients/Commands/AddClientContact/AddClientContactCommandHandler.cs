using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Clients.Commands.AddClientContact
{
    public class AddClientContactCommandHandler : IRequestHandler<AddClientContactCommand>
    {
        private readonly IGearContext _context;

        private readonly IUserAccessor _userAccessor;

        private readonly IMediator _mediator;

        public AddClientContactCommandHandler(IGearContext context, IUserAccessor userAccessor, IMediator mediator)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(AddClientContactCommand request, CancellationToken cancellationToken)
        {
/*
            var user = _userAccessor.GetUser();

            var contact = new ClientContact(request.ContactInfo,
                request.OrganizationId, request.Name, request.Id,
                Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value));

            var organization = await _context.ClientOrganizations.Include(x => x.Contacts)
                .FirstAsync(x => x.Id == request.OrganizationId, cancellationToken: cancellationToken);

            var addContactBusinessRule = organization.AddContact(contact, Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value));

            if (addContactBusinessRule.IsFailure) throw new BusinessUseCaseException(new List<string>() { addContactBusinessRule.Error });

            await _context.SaveChangesAsync(cancellationToken);


            await _mediator.Publish(new ClientContactAdded()
            {
                PrimaryEntityId = contact.Id,
                GroupEntityId = organization.Id,
                GroupEntityName = organization.Name,
                PrimaryEntityName = contact.Name.ToString(),
                UserName = user.Identity.Name
            }, cancellationToken);
*/

            return Unit.Value;
        }
    }
}
