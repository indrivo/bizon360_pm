using System;
using Gear.Common.Entities;

namespace Gear.Domain.PmEntities.Wiki
{
    public class Section : BaseModel
    {
        public string Content { get; set; }

        public Guid HeadlineId { get; set; }

        public Headline Headline { get; set; }
    }
}
