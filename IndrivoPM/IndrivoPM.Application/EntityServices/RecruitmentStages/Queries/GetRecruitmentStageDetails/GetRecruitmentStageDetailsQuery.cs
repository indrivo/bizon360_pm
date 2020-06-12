using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.RecruitmentStages.Queries.GetRecruitmentStageDetails
{
    public class GetRecruitmentStageDetailsQuery : IRequest<RecruitmentStageDetailsModel>
    {
        public Guid Id { get; set; }
    }
}
