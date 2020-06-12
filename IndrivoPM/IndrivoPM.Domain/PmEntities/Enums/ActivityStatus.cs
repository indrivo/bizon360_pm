using System.ComponentModel.DataAnnotations;

namespace Gear.Domain.PmEntities.Enums
{
    public enum ActivityStatus
    {
        New = 1,
        [Display(Name = "In progress")]
        InProgress = 2,
        Refused = 3,
        Developed = 4,
        Tested = 5,
        Completed = 6
    }
}
