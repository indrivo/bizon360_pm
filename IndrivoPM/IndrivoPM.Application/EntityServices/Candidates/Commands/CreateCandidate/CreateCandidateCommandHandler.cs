using System;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.HrmEntities.Recruitment;
using Gear.Domain.Infrastructure;
using Gear.Domain.ValueObjects;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.CreateCandidate
{
    internal class CreateCandidateCommandHandler : IRequestHandler<CreateCandidateCommand, Unit>
    {
        private readonly IMediator _mediator;
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="context"></param>
        /// <param name="userAccessor"></param>
        public CreateCandidateCommandHandler(IMediator mediator,
            IGearContext context,
            IUserAccessor userAccessor)
        {
            _mediator = mediator;
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(CreateCandidateCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            //TODO:Import the finder extension from Renson projects
            var job = await _context.JobPositions.FindAsync(request.JobPositionId);

            var stage = await _context.RecruitmentStages.FindAsync(request.RecruitmentStageId);

            var entity = new Candidate(CompoundName.Create(request.FirstName,request.LastName).Value, job,
                ContactInfo.Create(request.PhoneNumber,request.Email).Value);

            var operation = stage.AddCandidate(entity);

            if(operation.IsFailure) throw new InvalidOperationException(operation.Error);

            _context.RecruitmentStages.Update(stage);
            await _context.SaveChangesAsync(cancellationToken);

            // Notifications
            var notification = new CandidateCreated
            {
                PrimaryEntityId = entity.Id,
                UserName = user.Identity.Name
            };
            await _mediator.Publish(notification, cancellationToken);

            return Unit.Value;
        }
    }
}
