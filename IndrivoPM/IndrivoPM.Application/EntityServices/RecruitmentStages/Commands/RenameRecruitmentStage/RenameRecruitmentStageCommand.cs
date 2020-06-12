using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitmentStages.Commands.RenameRecruitmentStage
{
    public class RenameRecruitmentStageCommand : IRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
