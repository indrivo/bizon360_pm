using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Gear.Domain.HrmEntities;
using Gear.Domain.HrmEntities.Enums;
using Gear.Domain.HrmEntities.Recruitment;

namespace Gear.Manager.Core.EntityServices.Candidates.Queries.GetCandidateDetails
{
    public class CandidateDetailsModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public Guid? DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public IEnumerable<Guid> TagIds { get; set; }

        public string Description { get; set; }

        public CandidateStatus CandidateStatus { get; set; }

        public JobPosition JobPosition { get; set; }

        public Guid? RecruitmentStageId { get; set; }

        public Tuple<Guid, string> RecruitmentStage { get; set; }

        public Tuple<Guid, string> RecruitingPipeline { get; set; }

        public decimal? DesiredSalary { get; set; }

        public DateTime CreatedTime { get; set; }

        public static Expression<Func<Candidate, CandidateDetailsModel>> Projection
        {
            get
            {
                return candidate => new CandidateDetailsModel
                {
                    Id = candidate.Id,
                    CandidateStatus = candidate.CandidateStatus,
                    Description = candidate.Description,
                    JobPosition = candidate.JobPosition,
                    RecruitmentStage = candidate.RecruitmentStage != null ?
                        new Tuple<Guid, string>(candidate.RecruitmentStage.Id, 
                        candidate.RecruitmentStage.Name)
                        : new Tuple<Guid, string>(Guid.Empty, string.Empty),
                    RecruitingPipeline = candidate.RecruitmentStage != null &&
                        candidate.RecruitmentStage.Pipeline != null ?
                        new Tuple<Guid, string>(candidate.RecruitmentStage.Pipeline.Id,
                        candidate.RecruitmentStage.Pipeline.Name)
                        : new Tuple<Guid, string>(Guid.Empty, string.Empty),
                    DesiredSalary = candidate.DesiredSalary,
                    CreatedTime = candidate.CreatedTime
                };
            }
        }
        
        public static CandidateDetailsModel Create(Candidate candidate)
        {
            return Projection.Compile().Invoke(candidate);
        }
    }
}
