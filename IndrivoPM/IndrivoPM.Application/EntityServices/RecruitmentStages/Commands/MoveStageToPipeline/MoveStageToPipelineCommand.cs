using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitmentStages.Commands.MoveStageToPipeline
{
    public class MoveStageToPipelineCommand : IRequest
    {
        public Guid RecruitingPipelineId { get; set; }
        
        [Display(Name="Stages")]
        public List<Guid> StagesId { get; set; }
    }
}
