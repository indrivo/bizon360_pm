using System.ComponentModel.DataAnnotations;

namespace Gear.Domain.PmEntities.Enums
{
    public enum MaxLogHour
    {
        [Display(Name = "4.00")]
        FourHours = 4,

        [Display(Name = "6.00")]
        SixHours = 6,

        [Display(Name = "8.00")]
        EightHours = 8,

        [Display(Name = "10.00")]
        TenHours = 10,

        [Display(Name = "12.00")]
        TwelveHours = 12,

        [Display(Name = "No restriction")]
        NoRestriction = 0
    }
}
