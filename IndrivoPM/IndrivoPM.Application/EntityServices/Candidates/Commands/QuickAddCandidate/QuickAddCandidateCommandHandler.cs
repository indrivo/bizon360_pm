using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.HrmEntities.Recruitment;
using Gear.Domain.Infrastructure;
using Gear.Domain.ValueObjects;
using Gear.Manager.Core.EntityServices.Candidates.Commands.CreateCandidate;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.QuickAddCandidate
{
    public class QuickAddCandidateCommandHandler : IRequestHandler<QuickAddCandidateCommand, Unit>
    {
        private readonly IMediator _mediator;
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mediator"></param>
        /// <param name="userAccessor"></param>
        public QuickAddCandidateCommandHandler(IGearContext context, IMediator mediator, IUserAccessor userAccessor)
        {
            _context = context;
            _mediator = mediator;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(QuickAddCandidateCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var job = await _context.JobPositions.FindAsync(request.JobPositionId);

            var entity = new Candidate(CompoundName.Create(request.FirstName,request.LastName).Value,job,
                ContactInfo.Create(request.PhoneNumber,request.Email).Value);
            var stage = await _context.RecruitmentStages.FindAsync(request.RecruitmentStageId);
            stage.AddCandidate(entity);

            
            _context.RecruitmentStages.Update(stage);
            await _context.SaveChangesAsync(cancellationToken);

            // Notifications
            var notification = new CandidateCreated
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
