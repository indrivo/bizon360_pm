using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Wiki;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Sections.CreateSection
{
    public class CreateSectionCommandHandler : IRequestHandler<CreateSectionCommand, Unit>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public CreateSectionCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(CreateSectionCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var entity = new Section
            {
                Content = request.Content,
                HeadlineId = request.HeadlineId
            };

            entity.CreateEnd(request.Id, request.Title,
                Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value));

            await _context.Sections.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
