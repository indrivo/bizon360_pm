using System.ComponentModel.DataAnnotations;

namespace Gear.Domain.PmEntities.Enums
{
    public enum ActivityListStatus
    {
        New,
        [Display(Name="Ongoing")]
        OnGoing,
        Completed
    }
}
