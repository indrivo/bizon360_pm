using System.Collections.Generic;
using Gear.Common.Entities;

namespace Gear.Domain.PmEntities
{
    public class ProjectGroup : BaseModel
    {
        public ICollection<Project> Projects { get; set; }
    }
}
