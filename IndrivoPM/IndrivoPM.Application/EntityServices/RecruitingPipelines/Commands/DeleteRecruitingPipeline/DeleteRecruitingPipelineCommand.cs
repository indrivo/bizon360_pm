using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitingPipelines.Commands.DeleteRecruitingPipeline
{
    public class DeleteRecruitingPipelineCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
