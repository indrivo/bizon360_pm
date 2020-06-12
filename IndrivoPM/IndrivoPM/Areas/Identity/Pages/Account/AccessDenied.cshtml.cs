using Bizon360.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bizon360.Areas.Identity.Pages.Account
{
    public class AccessDeniedModel : PageModel
    {
        public void OnGet()
        {
            ViewData["Platform"] = Platform.Pm;
        }
    }
}

