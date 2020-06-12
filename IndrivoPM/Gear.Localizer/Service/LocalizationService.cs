using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gear.Localizer.Extensions;
using Gear.Localizer.Models;
using Gear.Localizer.Service.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Gear.Localizer.Service
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IOptionsSnapshot<LocalizationConfig> _locConfig;
        private readonly IHostingEnvironment _env;
        private readonly IStringLocalizer _localizer;

        public LocalizationService(IOptionsSnapshot<LocalizationConfig> locConfig, IHostingEnvironment env,
            IStringLocalizer localizer)
        {
            _locConfig = locConfig;
            _env = env;
            _localizer = localizer;
        }

        /// <summary>
        /// Add language
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddLanguage(AddLanguageViewModel model)
        {
            var existsInConfig =
                _locConfig.Value.Languages.Any(m => m.Identifier == model.Identifier && m.Name == model.Name);

            var cPaths = new[]
            {
                _env.ContentRootPath,
                _locConfig.Value.Path,
                $"{model.Identifier}.json"
            };

            var filePath = Path.Combine(cPaths);
            var fileExists = File.Exists(filePath);

            if (existsInConfig || fileExists)
            {
                return false;
            }

            using (Stream stream =
                new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
            using (var sWriter = new StreamWriter(stream))
            using (var writer = new JsonTextWriter(sWriter))
            {
                var keys = _localizer.GetAllForLanguage("en").Select(ls => ls.Name);
                var dict = keys.ToDictionary<string, string, string>(key => key, key => null);
                var obj = JObject.FromObject(dict);

                writer.Formatting = Formatting.Indented;
                obj.WriteTo(writer);
            }

            var langFile = _env.ContentRootFileProvider.GetFileInfo("appsettings.json");

            using (Stream str = new FileStream(langFile.PhysicalPath, FileMode.Open, FileAccess.Read,
                FileShare.ReadWrite))
            using (var sReader = new StreamReader(str))
            using (var reader = new JsonTextReader(sReader))
            {
                var fileObj = JObject.Load(reader);
                _locConfig.Value.Languages.Add(new LanguageCreateViewModel
                {
                    Identifier = model.Identifier,
                    Name = model.Name,
                    IsDisabled = false
                });
                var newLangs = JArray.FromObject(_locConfig.Value.Languages);
                fileObj[nameof(LocalizationConfig)][nameof(LocalizationConfig.Languages)] = newLangs;
                reader.Close();
                File.WriteAllText(langFile.PhysicalPath, fileObj.ToString());
            }

            return true;
        }

        public bool ChangeStatusOfLanguage(LanguageCreateViewModel model)
        {
            throw new NotImplementedException();
        }

        ///// <summary>
        ///// Change statu of language
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public bool ChangeStatusOfLanguage(LanguageCreateViewModel model)
        //{
        //    var existsInConfig =
        //        _locConfig.Value.Languages.Any(m => m.Identifier == model.Identifier && m.Name == model.Name);

        //    var cPaths = new[]
        //    {
        //        _env.ContentRootPath,
        //        _locConfig.Value.Path,
        //        $"{model.Identifier}.json"
        //    };

        //    var filePath = Path.Combine(cPaths);
        //    var fileExists = File.Exists(filePath);

        //    if (existsInConfig || fileExists)
        //    {
        //        var langFile = _env.ContentRootFileProvider.GetFileInfo("appsettings.json");
        //        Console.WriteLine(langFile);
        //        using (Stream str = new FileStream(langFile.PhysicalPath, FileMode.Open, FileAccess.Read,
        //            FileShare.ReadWrite))
        //        using (var sReader = new StreamReader(str))
        //        using (var reader = new JsonTextReader(sReader))
        //        {
        //            var fileObj = JObject.Load(reader);
        //            var languages = _locConfig.Value.Languages;
        //            var updLangs = new HashSet<LanguageCreateViewModel>();
        //            foreach (var e in languages)
        //            {
        //                if (e.Identifier == model.Identifier)
        //                {
        //                    updLangs.Add(new LanguageCreateViewModel
        //                    {
        //                        Identifier = model.Identifier,
        //                        Name = model.Name,
        //                        IsDisabled = model.IsDisabled
        //                    });
        //                }
        //                else
        //                {
        //                    updLangs.Add(new LanguageCreateViewModel
        //                    {
        //                        Identifier = e.Identifier,
        //                        Name = e.Name,
        //                        IsDisabled = false
        //                    });
        //                }
        //            }

        //            var updatedLangs = JObject
        //                .FromObject(new LocalizationConfig
        //                {
        //                    DefaultLanguage = _locConfig.Value.DefaultLanguage,
        //                    Languages = updLangs,
        //                    Path = _locConfig.Value.Path,
        //                    SessionStoreKeyName = _locConfig.Value.SessionStoreKeyName
        //                });

        //            fileObj[nameof(LocalizationConfig)] = updatedLangs;
        //            reader.Close();
        //            File.WriteAllText(langFile.PhysicalPath, fileObj.ToString());
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        /// <summary>
        /// Edit key
        /// </summary>
        /// <param name="model"></param>
        public void EditKey(EditLocalizationViewModel model)
        {
            var newStrings = model.LocalizedStrings;
            AddOrUpdateKey(model.Key, newStrings);
        }

        /// <summary>
        /// Update keys
        /// </summary>
        /// <param name="key"></param>
        /// <param name="localizedStrings"></param>
        public void AddOrUpdateKey(string key, IDictionary<string, string> localizedStrings)
        {
            foreach (var localized in localizedStrings)
            {
                var filePath = Path.Combine(_env.ContentRootPath, _locConfig.Value.Path, localized.Key + ".json");
                var cacheKey = $"{_locConfig.Value.SessionStoreKeyName}_{localized.Key}_{key}";
                if (!File.Exists(filePath))
                {
                    using (Stream str = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write,
                        FileShare.Write))
                    using (var sWriter = new StreamWriter(str))
                    using (var writer = new JsonTextWriter(sWriter))
                    {
                        writer.Formatting = Formatting.Indented;
                        writer.WriteStartObject();
                        writer.WritePropertyName(key);
                        writer.WriteValue(localized.Value);
                        writer.WriteEndObject();
                    }
                }
                else
                {
                    using (Stream stream =
                        new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var sReader = new StreamReader(stream))
                    using (var reader = new JsonTextReader(sReader))
                    {
                        var jobj = JObject.Load(reader);
                        jobj[key] = localized.Value;
                        reader.Close();
                        File.WriteAllText(filePath, jobj.ToString());
                    }
                }
            }
        }
    }
}