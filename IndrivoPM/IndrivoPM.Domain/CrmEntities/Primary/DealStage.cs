using System;
using System.Collections.Generic;
using System.Linq;
using Gear.Common.Entities;

namespace Gear.Domain.CrmEntities.Primary
{
    public class DealStage : BaseModel
    {
        public ushort OrderInsidePipeline { get; private set; } = 0;

        public Guid PipelineId { get; private set; }

        public DealPipeline Pipeline { get; private set; }


        private HashSet<Deal> _deals { get; set; }

        public IEnumerable<Deal> Deals()
        {
            return _deals?.ToList();
        }


        [Obsolete("Only for model binders and EF, don't use it in your code", true)]
        private DealStage()
        {
            //
        }

        public DealStage(Guid id, string name, 
            Guid pipelineId, short orderInsidePipeline, Guid createdBy)
        {
            if(pipelineId == Guid.Empty)
                throw new InvalidOperationException("Pipeline Id empty");

            if (orderInsidePipeline == 0)
            {
                throw new ArgumentException("Order must be more than 0");
            }

            this.CreateEnd(id,name,createdBy);
        }
    }
}