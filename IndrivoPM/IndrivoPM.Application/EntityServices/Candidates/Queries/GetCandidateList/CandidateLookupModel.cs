using System;
using System.Linq.Expressions;
using Gear.Domain.HrmEntities.Enums;
using Gear.Domain.HrmEntities.Recruitment;

namespace Gear.Manager.Core.EntityServices.Candidates.Queries.GetCandidateList
{
    public class CandidateLookupModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public decimal DesiredSalary { get; set; }

        public string Description { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid RecruitingStageId { get; set; }

        public Guid? ColorTagId { get; set; }

        public string Tag { get; set; }

        public CandidateStatus CandidateStatus { get; set; }

        public string JobPositionName { get; set; }

        public DateTime CreatedTime { get; set; }

        public bool Active { get; set; }

        public static Expression<Func<Candidate, CandidateLookupModel>> Projection
        {
            get
            {
                return candidate => new CandidateLookupModel
                {
                    Id = candidate.Id,
                    Name = candidate.Name.GetFullName,
                    Active = candidate.Active,
                    //ColorTagId = candidate.ColorTagId,
                    CandidateStatus = candidate.CandidateStatus,
                    JobPositionName = candidate.JobPosition != null && candidate.JobPosition.Name != null
                        ? candidate.JobPosition.Name
                        : "None",
                    Email = candidate.ContactInfo.Email,
                    PhoneNumber = candidate.ContactInfo.PhoneNumber,
                    Tag = candidate.RecruitmentStage != null && candidate.RecruitmentStage.Pipeline != null ?
                        candidate.RecruitmentStage.Pipeline.Name ?? "None" : "None",
                    CreatedTime = candidate.CreatedTime
                };
            }
        }

        public static CandidateLookupModel Create(Candidate candidate)
        {
            return Projection.Compile().Invoke(candidate);
        }
    }
}
