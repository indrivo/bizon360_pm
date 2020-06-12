using System.Collections.Generic;
using System.Linq;
using Gear.Common.Entities;
using Gear.Common.Extensions.Result;

namespace Gear.Domain.HrmEntities.Recruitment
{
    public class RecruitmentPipeline : BaseModel
    {
        public RecruitmentPipeline()
        {
        }

        public string Description { get; set; } = "";

        private HashSet<RecruitmentStage> _recruitmentStages;

        public IEnumerable<RecruitmentStage> RecruitmentStages => _recruitmentStages?.ToList();

        /// <summary>
        /// Create new stage in the pipeline.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Result AddStage(string name)
        {
            //TODO: after connecting the Common module throw exception on null collection.

            var stage = new RecruitmentStage(name, this);

            if (_recruitmentStages.Any(x => x.Name == name)) return Result.Fail("Stage already present");

            _recruitmentStages.Add(stage);

            return Result.Ok();
        }

        /// <summary>
        /// Delete stage from pipeline.
        /// </summary>
        /// <param name="stage"></param>
        /// <returns></returns>
        public Result RemoveStage(RecruitmentStage stage)
        {
            //TODO: after connecting the Common module throw exception on null collection.

            if (!_recruitmentStages.Contains(stage)) return Result.Fail("Stage is not existent");

            _recruitmentStages.Remove(stage);

            return Result.Ok();
        }

        /// <summary>
        /// Move candidate from a stage to another one in the same pipeline.
        /// </summary>
        /// <param name="candidate"></param>
        /// <param name="fromStage"></param>
        /// <param name="toStage"></param>
        /// <returns></returns>
        public Result MoveCandidateToStage(Candidate candidate, RecruitmentStage fromStage, RecruitmentStage toStage)
        {
            fromStage.RemoveCandidateFromStage(candidate);
            
            toStage.AddCandidate(candidate);
            
            return Result.Ok();
        }
    }
}
