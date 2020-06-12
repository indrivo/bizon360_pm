using System;
using Gear.Domain.PmEntities.Enums;

namespace Bizon360.Models
{
    public class SprintViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public SprintStatus Status { get; set; }

        public DateTime DueDate { get; set; }
    }
}
