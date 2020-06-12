using System;

namespace Gear.Domain.ReportEntities
{
    public class ServiceTimeChecker
    {
        public Guid ServiceId { get; set; }

        public string ServiceName { get; set; }

        public DateTime? ExecutedLastTime { get; set; }
    }
}
