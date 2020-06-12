using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Wiki;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Headlines.CreateHeadline
{
    public class CreateHeadlineCommandHandler : IRequestHandler<CreateHeadlineCommand, Unit>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public CreateHeadlineCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(CreateHeadlineCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value);

            var entity = new Headline
            {
                ProjectId = request.ProjectId
            };

            entity.CreateEnd(request.Id, request.Title, currentUserId);

            await _context.WikiHeadlines.AddAsync(entity, cancellationToken);

            if (!string.IsNullOrEmpty(request.SectionBody) || !string.IsNullOrEmpty(request.SectionName))
            {
                var section = new Section
                {
                    HeadlineId = entity.Id,
                    Content = request.SectionBody
                };

                section.CreateEnd(Guid.NewGuid(), request.SectionName, currentUserId);

                await _context.Sections.AddAsync(section, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
