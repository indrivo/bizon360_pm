using System.Linq;
using AutoMapper;
using Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserList;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace Bizon360.Controllers
{
    public abstract partial class BaseController : Controller
    {
        private IMapper _mapper;
        private IMediator _mediator;

        protected IMapper Mapper => _mapper ?? (_mapper = HttpContext.RequestServices.GetService<IMapper>());
        protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());

        /// <summary>
        /// Set ViewBag for actions
        /// Set global ViewBags for the controllers
        /// Can impact use time in large projects
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var users = Mediator.Send(new GetApplicationUserListQuery()).Result.ApplicationUsers.Where(x => x.Active).ToList();
            if (users != null)
            {
                ViewBag.ApplicationUsers = users.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.FullName
                }).ToList();
            }

            ViewBag.DarkTheme = Request.Cookies["DarkTheme"] ?? "false";
        }
    }
}