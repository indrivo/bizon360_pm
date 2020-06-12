using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Wiki;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Sections.UpdateSection
{
    public class UpdateSectionCommandHandler : IRequestHandler<UpdateSectionCommand, Unit>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public UpdateSectionCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(UpdateSectionCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var entity = await _context.Sections.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Section), request.Id);
            }

            entity.Name = request.Title;
            entity.Content = request.Content;
            

            _context.Sections.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
