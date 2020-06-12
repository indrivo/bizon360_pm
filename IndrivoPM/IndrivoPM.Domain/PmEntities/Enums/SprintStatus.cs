using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Gear.Domain.PmEntities.Enums
{
    public enum SprintStatus
    {
        New,
        [Display(Name = "Ongoing")]
        OnGoing,
        Completed
    }
}
