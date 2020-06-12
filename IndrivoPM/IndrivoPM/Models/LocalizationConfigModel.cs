using System.Collections.Generic;
using Gear.Localizer.Models;

namespace Bizon360.Models
{
    public class LocalizationConfigModel
    {
        public HashSet<LanguageCreateViewModel> Languages { get; set; }
        public string Path { get; set; }
        public string SessionStoreKeyName { get; set; }
        public string DefaultLanguage { get; set; }
    }
}