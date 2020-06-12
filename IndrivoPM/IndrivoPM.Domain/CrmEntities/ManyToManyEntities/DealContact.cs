using System;
using Gear.Domain.CrmEntities.Primary;

namespace Gear.Domain.CrmEntities.ManyToManyEntities
{
    public class DealContact
    {
        public Guid ContactId { get; set; }

        public ClientContact ClientContact { get; set; }

        public Guid DealId { get; set; }

        public Deal Deal { get; set; }
    }
}
