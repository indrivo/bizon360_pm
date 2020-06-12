using System;

namespace Bizon360.Models
{
    public class ActivityListViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid ProjectId { get; set; }

        public string ProjectName { get; set; }
    }
}
