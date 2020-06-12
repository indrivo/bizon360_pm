using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.HrmEntities.Recruitment;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitmentStages.Commands.RenameRecruitmentStage
{
    public class RenameRecruitmentStageCommandHandler : IRequestHandler<RenameRecruitmentStageCommand, Unit>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public RenameRecruitmentStageCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(RenameRecruitmentStageCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.RecruitmentStages.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(RecruitmentStage), request.Id);
            }

            entity.Name = request.Name;

            _context.RecruitmentStages.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
