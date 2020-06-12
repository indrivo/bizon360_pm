using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.BulkActions.BulkMoveCandidateToStage
{
    public class BulkMoveCandidateToStageCommand : IRequest
    {
        [Display(Name = "Stage")]
        public Guid StageId { get; set; }

        [Display(Name = "Candidates")]
        public List<Guid> CandidatesId { get; set; }
    }
}
