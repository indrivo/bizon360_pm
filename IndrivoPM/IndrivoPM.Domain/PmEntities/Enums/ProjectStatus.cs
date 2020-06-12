using System.ComponentModel.DataAnnotations;

namespace Gear.Domain.PmEntities.Enums
{
    public enum ProjectStatus
    {
        New = 1,
        [Display(Name = "In progress")]
        InProgress = 2,
        [Display(Name = "On hold")]
        OnHold = 3,
        Canceled = 4,
        Completed = 5
    }
}
