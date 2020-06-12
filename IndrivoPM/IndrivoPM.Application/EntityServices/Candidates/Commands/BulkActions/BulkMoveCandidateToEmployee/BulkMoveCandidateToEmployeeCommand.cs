using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.BulkActions.BulkMoveCandidateToEmployee
{
    public class BulkMoveCandidateToEmployeeCommand : IRequest
    {
        [Display(Name = "Candidates")]
        public List<Guid> CandidatesId { get; set; }
    }
}
