using System.Collections.Generic;

namespace Gear.Localizer.Extensions
{
    public class LocalizationConfig
    {
        /// <summary>
        /// These backing fields will have default values in case
        /// the json configuration for languages does not exist.
        /// </summary>

        #region Backing fields Default Values

        private HashSet<Language> _defaultLanguages = new HashSet<Language>(new LanguageEqualityComparer())
        {
            new Language {Identifier = "en", Name = "English"}
        };

        #endregion

        public HashSet<Language> Languages
        {
            get => _defaultLanguages;
            set => _defaultLanguages = value;
        }

        /// <summary>
        /// Path where the json localization files are located
        /// </summary>
        public string Path { get; set; } = "Localization";

        /// <summary>
        /// Name of the key in Session that stores the
        /// current selected language.
        /// </summary>
        public string SessionStoreKeyName { get; set; } = "lang";

        /// <summary>
        /// Default language used in localization
        /// </summary>
        public string DefaultLanguage { get; set; } = "en";
    }
}