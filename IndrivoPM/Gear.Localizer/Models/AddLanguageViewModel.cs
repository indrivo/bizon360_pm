using System.ComponentModel.DataAnnotations;

namespace Gear.Localizer.Models
{
    public class AddLanguageViewModel
    {
        [Required] public string Identifier { get; set; }

        [Required] public string Name { get; set; }
    }
}