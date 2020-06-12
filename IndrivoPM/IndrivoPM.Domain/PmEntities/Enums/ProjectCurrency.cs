
using System.ComponentModel.DataAnnotations;

namespace Gear.Domain.PmEntities.Enums
{
    public enum ProjectCurrency
    {
        [Display(Name = "EUR (€)")]
        EUR,

        [Display(Name = "USD ($)")]
        USD,

        RON,
        MDL
    }
}
