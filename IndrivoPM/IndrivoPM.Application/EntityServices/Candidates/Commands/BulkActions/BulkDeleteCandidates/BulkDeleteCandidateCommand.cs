using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.BulkActions.BulkDeleteCandidates
{
    public class BulkDeleteCandidateCommand : IRequest
    {
        public ICollection<Guid> Candidates { get; set; }
    }
}
