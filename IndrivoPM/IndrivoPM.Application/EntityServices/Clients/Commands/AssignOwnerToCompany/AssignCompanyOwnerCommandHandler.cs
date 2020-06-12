using System;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Clients.Commands.AssignOwnerToCompany
{
    public class AssignCompanyOwnerCommandHandler : IRequestHandler<AssignCompanyOwnerCommand>
    {
        private readonly IGearContext _context;

        private readonly IUserAccessor _userAccessor;

        private readonly IMediator _mediator;

        public AssignCompanyOwnerCommandHandler(IGearContext context, IUserAccessor userAccessor, IMediator mediator)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mediator = mediator;
        }

        public Task<Unit> Handle(AssignCompanyOwnerCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
