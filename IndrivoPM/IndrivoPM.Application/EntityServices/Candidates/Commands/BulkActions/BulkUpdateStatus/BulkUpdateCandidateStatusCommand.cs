using System;
using System.Collections.Generic;
using Gear.Domain.HrmEntities.Enums;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.BulkActions.BulkUpdateStatus
{
    public class BulkUpdateCandidateStatusCommand : IRequest
    {
        public ICollection<Guid> Candidates { get; set; }

        public CandidateStatus Status { get; set; }
    }
}
