using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.DeleteRecruitmentStage
{
    public class DeleteRecruitmentStageCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
