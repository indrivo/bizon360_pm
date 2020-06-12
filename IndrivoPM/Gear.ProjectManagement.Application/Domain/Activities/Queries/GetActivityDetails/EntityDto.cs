using System;
using System.Collections.Generic;
using Gear.Common.Entities;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityDetails
{
    public class EntityDto
    {
        public string Action { get; set; }

        public Entity Entity { get; set; }

        public List<EntryChange> Changes { get; set; }
    }

    public class Entity
    {
        public string Name { get; set; }

        public Guid ModifiedBy { get; set; }

        public DateTime ModifiedTime { get; set; }
    }
}
