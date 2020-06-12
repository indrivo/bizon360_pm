using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitmentStages.Commands.UpdateRecruitmentStage
{
    public class UpdateRecruitmentStageCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid RecruitingPipelineId { get; set; }
    }
}
