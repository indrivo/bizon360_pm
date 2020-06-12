using System;
using Gear.Common.Entities;
using Gear.Common.Extensions.Result;
using Gear.Domain.HrmEntities.Enums;
using Gear.Domain.ValueObjects;

namespace Gear.Domain.HrmEntities.Recruitment
{
    /// <summary>
    /// Candidate for specific position.
    /// </summary>
    public class Candidate : BaseModel
    {
        [Obsolete("Only for Ef core and mappers")]
        protected Candidate()
        {
        }

        public new CompoundName Name { get; private set; }

        public ContactInfo ContactInfo { get; private set; }

        public JobPosition JobPosition { get; private set; }

        /// <summary>
        /// Create a candidate and fill in the basic required information.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="jobPosition"></param>
        /// <param name="contactInfo"></param>
        public Candidate(CompoundName name, JobPosition jobPosition, ContactInfo contactInfo)
        {
            Name = name;
            JobPosition = jobPosition;
            ContactInfo = contactInfo;
        }

        public CandidateStatus CandidateStatus { get; private set; }

        /// <summary>
        /// Change candidate status.
        /// </summary>
        /// <param name="candidateStatus"></param>
        /// <returns></returns>
        public Result ChangeStatus(CandidateStatus candidateStatus)
        {
            if(CandidateStatus == candidateStatus) return Result.Fail("Cannot set status to the same one");

            //Here will go future validation of the statuses of candidates
            CandidateStatus = candidateStatus;

            return Result.Ok();
        }

        public decimal DesiredSalary { get; private set; }

        /// <summary>
        /// Update candidate desired salary.
        /// If some BUCases will require additional check it will go in here.
        /// </summary>
        /// <param name="salaryAmount"></param>
        /// <returns></returns>
        public Result ChangeSalary(decimal salaryAmount)
        {
            DesiredSalary = salaryAmount;
            return Result.Ok();
        }

        public string Description { get; private set; }

        /// <summary>
        /// Change Candidate description.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Result ChangeDescription(string description)
        {
            if(string.IsNullOrEmpty(description)) return Result.Fail("Cannot insert null or empty description");

            Description = description;
            return Result.Ok();
        }

        public RecruitmentStage RecruitmentStage { get; set; }
    }
}