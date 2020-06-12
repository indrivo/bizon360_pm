using System.Threading.Tasks;
using Gear.CloudStorage.Abstractions.Domain;
using Gear.CloudStorage.Abstractions.Service.Abstractions;
using Gear.Domain.AppEntities;
using Gear.Manager.Core.EntityServices.CloudStorage.Commands.RefreshUserToken;
using Gear.Manager.Core.EntityServices.CloudStorage.Commands.RegisterUserToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartBreadcrumbs.Attributes;
using Platform = Bizon360.Models.Platform;

namespace Bizon360.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IStorageBaseService _service;
        private readonly UserManager<ApplicationUser> _userManager;


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Platform"] = Platform.Pm;
        }

        public HomeController(IStorageBaseService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        [DefaultBreadcrumb("Dashboard")]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Projects");
        }

        public IActionResult LogInMicrosoft()
        {
            return Redirect(_service.GetCodeLoginUrl(
                "files.readwrite.all offline_access User.Read Sites.Read.All"));
        }

        public async Task<RedirectResult> OnAuthComplete(string code)
        {
            await Mediator.Send(new RegisterUserTokenCommand() {OAuthCode = code});
            return new RedirectResult("Index");
        }

        public ActionResult VerifyUserToken()
        {
            Mediator.Send(new RefreshCurrentUserTokenCommand() {ExternalProvider = ExternalProviders.OneDrive});
            return Ok();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string errCode)
        {
            ViewData["ErrorType"] = errCode;
            return View();
            //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
