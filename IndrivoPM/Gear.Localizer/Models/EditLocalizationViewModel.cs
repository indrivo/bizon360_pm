using System.Collections.Generic;

namespace Gear.Localizer.Models
{
    public class EditLocalizationViewModel
    {
        public string Key { get; set; }
        public Dictionary<string, string> LocalizedStrings { get; set; }
        public Dictionary<string, string> Languages { get; set; }
    }
}