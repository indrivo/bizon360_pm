using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.UpdateCandidate
{
    public class UpdateCandidateCommandHandler : IRequestHandler<UpdateCandidateCommand, Unit>
    {
        private readonly IGearContext _context;
        private readonly IMediator _mediator;
        private readonly IUserAccessor _userAccessor;

        public UpdateCandidateCommandHandler(IMediator mediator, 
            IGearContext context, 
            IUserAccessor userAccessor)
        {
            _context = context;
            _mediator = mediator;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(UpdateCandidateCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var entity = await _context.Candidates.FindAsync(request.Id);
            
            entity.ChangeStatus(request.CandidateStatus);


            entity.ChangeDescription(request.Description);
            entity.ChangeSalary(request.DesiredSalary ?? decimal.Zero);


            //TODO: Check this recruitment stage use case
            //if (request.RecruitmentStageId.HasValue)
            //{
            //    entity.RecruitmentStageId = request.RecruitmentStageId.Value;
            //}



            _context.Candidates.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            // Notifications
            var notification = new CandidateUpdated
            {
                PrimaryEntityId = entity.Id,
                PrimaryEntityName = entity.Name.GetFullName,
                UserName = user.Identity.Name
            };
            await _mediator.Publish(notification, cancellationToken);

            return Unit.Value;
        }
    }
}
