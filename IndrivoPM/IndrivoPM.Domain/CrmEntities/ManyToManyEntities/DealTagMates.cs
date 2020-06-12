using System;
using Gear.Domain.AppEntities;
using Gear.Domain.CrmEntities.Primary;

namespace Gear.Domain.CrmEntities.ManyToManyEntities
{
    public class DealTagMates
    {
        public Deal Deal { get; set; }

        public Guid DealId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public string ApplicationUserId { get; set; }
    }
}
