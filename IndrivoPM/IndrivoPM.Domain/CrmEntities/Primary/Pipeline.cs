using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gear.Common.Entities;
using Gear.Common.Extensions.Result;

namespace Gear.Domain.CrmEntities.Primary
{
    public class DealPipeline : BaseModel
    {
        private HashSet<DealStage> _stages { get; set; }

        public IEnumerable<DealStage> Stages => _stages?.ToList();


        [Obsolete("Only for model binders and EF, don't use it in your code", true)]
        private DealPipeline()
        {
            //   
        }

        public DealPipeline(Guid id, string name, Guid createdBy)
        {
            this._stages = new HashSet<DealStage>();
            this.CreateEnd(id,name,createdBy);
        }

        /// <summary>
        /// Add stage to pipeline
        /// </summary>
        /// <param name="stage"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public Result AddStage(DealStage stage, Guid modifiedBy)
        {
            //Check if Id's match
            if(this.Id != stage.PipelineId || stage.PipelineId == Guid.Empty)
                return Result.Fail("Cannot process, check organization Id");

            //Check if the collection contains element or not
            if (this.Stages != null )
                return Result.Fail("Please load the pipeline stages before execution");

            _stages.Add(stage);

            return Result.Ok();
        }

        /// <summary>
        /// Remove stage from pipeline
        /// </summary>
        /// <param name="stage"></param>
        /// <param name="modifiedBy"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Result RemoveStage(DealStage stage, Guid modifiedBy)
        {
            //Check if Id's match
            if (this.Id != stage.PipelineId || stage.PipelineId == Guid.Empty)
                return Result.Fail("Cannot process, check organization Id");

            //Check if the collection contains element or not
            if (this.Stages != null && this.Stages.Any(x => x.Name == stage.Name))
            {
                _stages.Remove(stage);
            }
            else
            {
                return Result.Fail("Please load the pipeline stages before execution");
            }

            //Remove stage
            stage.Deactivate();

            return Result.Ok();
        }

    }
}
