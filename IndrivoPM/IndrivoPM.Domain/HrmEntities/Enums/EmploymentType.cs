using System.ComponentModel.DataAnnotations;

namespace Gear.Domain.HrmEntities.Enums
{
    public enum EmploymentType
    {
        [Display(Name = "4.0 h (part-time)")]
        PartTime4H,
        [Display(Name = "6.0 h (part-time)")]
        PartTime6H,
        [Display(Name = "8.0 h (full-time)")]
        FullTime8H
    }
}