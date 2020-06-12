using System;

namespace Gear.Sstp.Abstractions.Domain
{
    public class SstpBaseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsDeletable { get; set; } = true;

        public bool Active { get; set; } = true;

        public int? RowOrder { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime ModifiedTime { get; set; }
    }
}
