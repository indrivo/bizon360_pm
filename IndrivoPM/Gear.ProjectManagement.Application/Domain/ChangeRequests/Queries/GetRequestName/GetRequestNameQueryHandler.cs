using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Queries.GetRequestName
{
    public class GetRequestNameQueryHandler : IRequestHandler<GetRequestNameQuery, string>
    {
        private readonly IGearContext _context;
        public GetRequestNameQueryHandler(IGearContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(GetRequestNameQuery request, CancellationToken cancellationToken)
        {

            var changeRequest = await _context.ChangeRequests.FindAsync(request.Id);

            if (changeRequest == null)
            {
                throw new NotFoundException(nameof(changeRequest), request.Id);
            }

            return changeRequest.Name;
        }
    }
}
