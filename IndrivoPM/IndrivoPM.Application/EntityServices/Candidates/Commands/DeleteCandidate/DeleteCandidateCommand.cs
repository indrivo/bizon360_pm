using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.DeleteCandidate
{
    public class DeleteCandidateCommand : IRequest
    {
        public Guid CandidateId { get; set; }
    }
}
