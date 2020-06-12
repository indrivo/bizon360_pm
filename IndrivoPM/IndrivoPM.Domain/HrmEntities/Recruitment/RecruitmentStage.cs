using System;
using System.Collections.Generic;
using System.Linq;
using Gear.Common.Entities;
using Gear.Common.Extensions.Result;

namespace Gear.Domain.HrmEntities.Recruitment
{
    public class RecruitmentStage : BaseModel
    {
        [Obsolete("Used for EF and other binders")]
        protected RecruitmentStage()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pipeline"></param>
        public RecruitmentStage(string name, RecruitmentPipeline pipeline)
        {
            Name = name;
            Pipeline = pipeline;
        }

        public RecruitmentPipeline Pipeline { get; private set; }

        private HashSet<Candidate> _candidates;

        public IEnumerable<Candidate> Candidates => _candidates?.ToList();

        /// <summary>
        /// Add candidate to the stage.
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        public Result AddCandidate(Candidate candidate)
        {
            //TODO: after connecting the Common module throw exception on null collection

            if (_candidates.Contains(candidate)) return Result.Fail("Candidate already present");

            //TODO: Check if the candidate stageId updates or a new one is added

            _candidates.Add(candidate);

            return Result.Ok();
        }

        /// <summary>
        /// Remove candidate from the stage.
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        public Result RemoveCandidateFromStage(Candidate candidate)
        {
            //TODO: after connecting the Common module throw exception on null collection

            if (!_candidates.Contains(candidate)) return Result.Fail("Candidate is not existent");

            _candidates.Remove(candidate);

            return Result.Ok();
        }

    }
}
