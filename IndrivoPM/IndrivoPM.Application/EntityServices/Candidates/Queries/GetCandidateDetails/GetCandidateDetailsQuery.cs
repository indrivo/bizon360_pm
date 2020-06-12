using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Queries.GetCandidateDetails
{
    public class GetCandidateDetailsQuery : IRequest<CandidateDetailsModel>
    {
        public Guid Id { get; set; }
    }
}
