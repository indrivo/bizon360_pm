using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.AddRecruitmentStage
{
    public class CreateRecruitmentStageCommand : IRequest
    {
        public string Name { get; set; }

        public Guid RecruitingPipelineId { get; set; }
    }
}
