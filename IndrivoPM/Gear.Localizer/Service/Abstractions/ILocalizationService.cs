using System.Collections.Generic;
using Gear.Localizer.Models;

namespace Gear.Localizer.Service.Abstractions
{
    public interface ILocalizationService
    {
        void EditKey(EditLocalizationViewModel model);
        void AddOrUpdateKey(string key, IDictionary<string, string> localizedStrings);
        bool AddLanguage(AddLanguageViewModel model);
        bool ChangeStatusOfLanguage(LanguageCreateViewModel model);
    }
}