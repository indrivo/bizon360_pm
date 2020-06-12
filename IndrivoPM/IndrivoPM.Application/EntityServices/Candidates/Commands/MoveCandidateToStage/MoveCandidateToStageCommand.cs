using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.MoveCandidateToStage
{
    public class MoveCandidateToStageCommand : IRequest
    {
        [Display(Name="Stage")]
        public Guid StageId { get; set; }

        [Display(Name="Candidate")]
        public Guid CandidateId { get; set; }
    }
}
