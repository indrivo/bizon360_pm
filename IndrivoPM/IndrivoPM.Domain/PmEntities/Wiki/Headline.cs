using System;
using System.Collections.Generic;
using Gear.Common.Entities;

namespace Gear.Domain.PmEntities.Wiki
{
    public class Headline : BaseModel
    {
        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        public ICollection<Section> Sections { get; set; }
    }
}
