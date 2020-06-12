using System.Collections.Generic;
using System.Linq;
using Bizon360.Models;
using Gear.Localizer.Extensions;
using Gear.Localizer.Models;
using Gear.Localizer.Service.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Bizon360.Controllers
{
    [Authorize]
    public class LocalizationController : Controller
    {
        private readonly IOptionsSnapshot<LocalizationConfigModel> _locConfig;
        private readonly IStringLocalizer _localizer;
        private readonly ILocalizationService _localizationService;

        public LocalizationController(IOptionsSnapshot<LocalizationConfigModel> locConfig, IStringLocalizer localizer,
            ILocalizationService localizationService)
        {
            _locConfig = locConfig;
            _localizer = localizer;
            _localizationService = localizationService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_locConfig.Value.Languages);
        }


        public IActionResult ChangeLanguage(string identifier, string returnUrl)
        {
            if (!string.IsNullOrEmpty(identifier))
            {
                var sessionKey = _locConfig.Value.SessionStoreKeyName;
                // Get the list of supported language identifiers.
                var lang = _locConfig.Value.Languages.Select(f => f.Identifier);
                // Check wether the provided language identifier is contained in the list of supported languages.
                // If the language is invalid then set to default language.
                var langIsValid = lang.Contains(identifier);
                HttpContext.Session.SetString(sessionKey, langIsValid ? identifier : _locConfig.Value.DefaultLanguage);
            }

            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public IActionResult EditKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return NotFound();
            }

            var rz = new Dictionary<string, string>();
            foreach (var item in _locConfig.Value.Languages)
            {
                var str = _localizer.GetForLanguage(key, item.Identifier);
                rz.Add(item.Identifier, str.Value != $"[{str.Name}]" ? str : string.Empty);
            }

            var model = new EditLocalizationViewModel
            {
                Key = key,
                LocalizedStrings = rz,
                Languages = _locConfig.Value.Languages.ToDictionary(f => f.Identifier, f => f.Name)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditKey(EditLocalizationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _localizationService.EditKey(model);
            return RedirectToAction("Index", "Localization");
        }


        [HttpGet]
        public IActionResult Translation(string lang)
        {
            if (string.IsNullOrEmpty(lang))
            {
                return NotFound();
            }

            var translations = JsonConvert.SerializeObject(_localizer.GetAllForLanguage(lang));
            var result = JsonConvert.DeserializeObject<IEnumerable<LocalizationListView>>(translations);

            ViewBag.lang = result;
            return View();
        }
    }
}