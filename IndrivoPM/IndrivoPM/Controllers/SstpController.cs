using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bizon360.Models;
using Gear.Identity.Permissions.Domain.Resources;
using Gear.Identity.Permissions.Infrastructure.Attributes;
using Gear.Sstp.Abstractions.Domain;
using Gear.Sstp.Abstractions.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartBreadcrumbs.Attributes;

namespace Bizon360.Controllers
{
    [Authorize]
    public class SstpController : Controller
    {
        private readonly ISstpRepo<ProductType> _productRepo;
        private readonly ISstpRepo<ServiceType> _serviceRepo;
        private readonly ISstpRepo<SolutionType> _solutionRepo;
        private readonly ISstpRepo<TechnologyType> _technologyRepo;

        public SstpController(
            ISstpRepo<ProductType> productRepo,
            ISstpRepo<ServiceType> serviceRepo,
            ISstpRepo<SolutionType> solutionRepo,
            ISstpRepo<TechnologyType> technologyType
            )
        {
            _productRepo = productRepo;
            _serviceRepo = serviceRepo;
            _solutionRepo = solutionRepo;
            _technologyRepo = technologyType;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["Platform"] = Platform.Adm;
            ViewBag.DarkTheme = Request.Cookies["DarkTheme"] ?? "false";
        }

        [Breadcrumb("ViewData.FirstNode", FromController = typeof(HomeController), FromAction = "Index")]
        [HasPermission(Permissions.SSTPRead)]
        public IActionResult Index()
        {
            ViewBag.ProductCount = _productRepo.GetAll().Count();
            ViewBag.ServiceCount = _serviceRepo.GetAll().Count();
            ViewBag.TechnologyCount = _technologyRepo.GetAll().Count();
            ViewBag.SolutionCount = _solutionRepo.GetAll().Count();

            return View();
        }

        #region ProductType

        [HttpGet]
        [HasPermission(Permissions.SSTPRead)]
        public ActionResult GetProductList()
        {
            return PartialView("_ProductList", _productRepo.GetAll().ToList());
        }
        
        [HttpGet]
        [HasPermission(Permissions.SSTPCreate)]
        public ActionResult CreateProduct()
        {
            return PartialView("_CreateProduct");
        }

        [HttpPost]
        [HasPermission(Permissions.SSTPCreate)]
        public async Task<ActionResult>  CreateProduct(ProductType product)
        {
            if (product == null) return BadRequest();

            await _productRepo.Create(product);

            return Ok();
        }

        [HttpPost]
        [HasPermission(Permissions.SSTPUpdate)]
        public async Task<ActionResult> ActivateProduct(List<Guid> id)
        {
            if (!await _productRepo.Activate(id)) return BadRequest();

            return Ok();
        }

        [HttpPost]
        [HasPermission(Permissions.SSTPUpdate)]
        public async Task<ActionResult> DeactivateProduct(List<Guid> id)
        {
            if (!await _productRepo.Deactivate(id)) return BadRequest();

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.SSTPUpdate)]
        public async Task<ActionResult> EditProduct(Guid id)
        {
            var product = await _productRepo.GetById(id);

           if (product == null) return BadRequest();

           return PartialView("_EditProduct", product);
        }

        [HttpPost]
        [HasPermission(Permissions.SSTPUpdate)]
        public async Task<ActionResult> EditProduct(ProductType product)
        {
           if (product == null) return BadRequest();

           await _productRepo.Update(product);

           return Ok();
        }

        [HttpPost]
        [HasPermission(Permissions.SSTPUpdate)]
        public async Task<ActionResult> UpdateProductOrder(List<Guid> itemIds)
        {
            if (itemIds.Count == 0) return BadRequest();

            await _productRepo.UpdateRowOrderAsync(itemIds);

            return new JsonResult(true);
        }

        [HasPermission(Permissions.SSTPDelete)]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
          await _productRepo.Delete(id);

           return Ok();
        }

        #endregion

        #region ServiceType
        [HttpGet]
        [HasPermission(Permissions.SSTPRead)]
        public ActionResult GetServiceList(string searchField)
        {
            if (searchField == null)
            {
                return PartialView("_ServiceList", _serviceRepo.GetAll().ToList());
            }

            var filterService = _serviceRepo.GetAll()
                .Where(x => x.Name.ToLower().Contains(searchField.ToLower())).ToList();

            return PartialView("_ServiceList", filterService);
        }

        [HttpGet]
        [HasPermission(Permissions.SSTPCreate)]
        public ActionResult CreateService()
        {
            return PartialView("_CreateService");
        }

        [HttpPost]
        [HasPermission(Permissions.SSTPCreate)]
        public async Task<ActionResult> CreateService(ServiceType service)
        {
            if (service == null)
            {
                return BadRequest();
            }

            await _serviceRepo.Create(service);

            return new JsonResult("Success");
        }

        [HttpGet]
        [HasPermission(Permissions.SSTPUpdate)]
        public ActionResult EditService(Guid id)
        {
            var service = _serviceRepo.GetById(id).Result;

            if (service == null)
            {
                return BadRequest();
            }

            return PartialView("_EditService", service);
        }

        [HttpPost]
        [HasPermission(Permissions.SSTPUpdate)]
        public async Task<ActionResult> ActivateService(List<Guid> id)
        {
            if (!await _serviceRepo.Activate(id)) return BadRequest();

            return Ok();
        }

        [HttpPost]
        [HasPermission(Permissions.SSTPUpdate)]
        public async Task<ActionResult> DeactivateService(List<Guid> id)
        {
            if (!await _serviceRepo.Deactivate(id)) return BadRequest();

            return Ok();
        }

        [HttpPost]
        [HasPermission(Permissions.SSTPUpdate)]
        public async Task<ActionResult> EditService(ServiceType service)
        {
            if (service == null)
            {
                return BadRequest();
            }

            await _serviceRepo.Update(service);

            return new JsonResult("Success");
        }

        [HttpPost]
        [HasPermission(Permissions.SSTPUpdate)]
        public async Task<ActionResult> UpdateServiceOrder(List<Guid> itemIds)
        {
            if (itemIds.Count == 0) return BadRequest();

            await _serviceRepo.UpdateRowOrderAsync(itemIds);

            return new JsonResult(true);
        }

        [HasPermission(Permissions.SSTPUpdate)]
        public async Task<ActionResult> DeleteService(Guid id)
        {
            await _serviceRepo.Delete(id);

            return new JsonResult("Success");
        }
        #endregion

        #region SolutionType
        [HttpGet]
        [HasPermission(Permissions.SSTPRead)]
        public ActionResult GetSolutionList(string searchField)
        {
            if (searchField == null)
            {
                return PartialView("_SolutionList", _solutionRepo.GetAll().ToList());
            }

            var filterSolution = _solutionRepo.GetAll()
                .Where(x => x.Name.ToLower().Contains(searchField.ToLower())).ToList();

            return PartialView("_SolutionList", filterSolution);
        }

        [HttpGet]
        [HasPermission(Permissions.SSTPCreate)]
        public ActionResult CreateSolution()
        {
            return PartialView("_CreateSolution");
        }

        [HttpPost]
        [HasPermission(Permissions.SSTPCreate)]
        public async Task<ActionResult> CreateSolution(SolutionType solution)
        {
            if (solution == null)
            {
                return BadRequest();
            }

            await _solutionRepo.Create(solution);

            return new JsonResult("Success");
        }

        [HttpPost]
        [HasPermission(Permissions.SSTPUpdate)]
        public async Task<ActionResult> ActivateSolution(List<Guid> id)
        {
            if (!await _solutionRepo.Activate(id)) return BadRequest();

            return Ok();
        }

        [HttpPost]
        [HasPermission(Permissions.SSTPUpdate)]
        public async Task<ActionResult> DeactivateSolution(List<Guid> id)
        {
            if (!await _solutionRepo.Deactivate(id)) return BadRequest();

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.SSTPUpdate)]
        public ActionResult EditSolution(Guid id)
        {
            var solution = _solutionRepo.GetById(id).Result;

            if (solution == null)
            {
                return BadRequest();
            }

            return PartialView("_EditSolution", solution);
        }

        [HttpPost]
        [HasPermission(Permissions.SSTPUpdate)]
        public async Task<ActionResult> EditSolution(SolutionType solution)
        {
            if (solution == null)
            {
                return BadRequest();
            }

            await _solutionRepo.Update(solution);

            return new JsonResult("Success");
        }

        [HttpPost]
        [HasPermission(Permissions.SSTPUpdate)]
        public async Task<ActionResult> UpdateSolutionOrder(List<Guid> itemIds)
        {
            if (itemIds.Count == 0) return BadRequest();

            await _solutionRepo.UpdateRowOrderAsync(itemIds);

            return new JsonResult(true);
        }

        [HasPermission(Permissions.SSTPDelete)]
        public async Task<ActionResult> DeleteSolution(Guid id)
        {
            await _solutionRepo.Delete(id);

            return new JsonResult("Success");
        }

        #endregion

        #region TechnologyType
        [HttpGet]
        [HasPermission(Permissions.SSTPRead)]
        public ActionResult GetTechnologyList(string searchField)
        {
            if (searchField == null) return PartialView("_TechnologyList", _technologyRepo.GetAll().ToList());

            var filterTechnology = _technologyRepo.GetAll()
                .Where(x => x.Name.ToLower().Contains(searchField.ToLower())).ToList();

            return PartialView("_TechnologyList", filterTechnology);
        }

        [HttpGet]
        [HasPermission(Permissions.SSTPCreate)]
        public ActionResult CreateTechnology()
        {
            return PartialView("_CreateTechnology");
        }

        [HttpPost]
        [HasPermission(Permissions.SSTPCreate)]
        public async Task<ActionResult> CreateTechnology(TechnologyType technology)
        {
            if (technology == null)
            {
                return BadRequest();
            }

            await _technologyRepo.Create(technology);

            return new JsonResult("Success");
        }

        [HttpPost]
        [HasPermission(Permissions.SSTPUpdate)]
        public async Task<ActionResult> ActivateTechnology(List<Guid> id)
        {
            if (!await _technologyRepo.Activate(id)) return BadRequest();

            return Ok();
        }

        [HttpPost]
        [HasPermission(Permissions.SSTPUpdate)]
        public async Task<ActionResult> DeactivateTechnology(List<Guid> id)
        {
            if (!await _technologyRepo.Deactivate(id)) return BadRequest();

            return Ok();
        }

        [HttpGet]
        [HasPermission(Permissions.SSTPUpdate)]
        public ActionResult EditTechnology(Guid id)
        {
            var technology = _technologyRepo.GetById(id).Result;

            if (technology == null)
            {
                return BadRequest();
            }

            return PartialView("_EditTechnology", technology);
        }

        [HttpPost]
        [HasPermission(Permissions.SSTPUpdate)]
        public async Task<ActionResult> EditTechnology(TechnologyType technology)
        {
            if (technology == null)
            {
                return BadRequest();
            }

            await _technologyRepo.Update(technology);

            return new JsonResult("Success");
        }

        [HasPermission(Permissions.SSTPDelete)]
        public async Task<ActionResult> DeleteTechnology(Guid id)
        {
            await _technologyRepo.Delete(id);

            return new JsonResult("Success");
        }

        [HasPermission(Permissions.SSTPUpdate)]
        public async Task<ActionResult> UpdateTechnologyOrder(List<Guid> itemIds)
        {
            if (itemIds.Count == 0) return BadRequest();

            await _technologyRepo.UpdateRowOrderAsync(itemIds);

            return new JsonResult(true);
        }

        #endregion
    }
}