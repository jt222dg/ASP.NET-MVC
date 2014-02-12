using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MapAdminInterface.Models;
using MapAdminInterface.ViewModels;

namespace MapAdminInterface.Controllers
{
    public class LocationTypeController : Controller
    {
        private LocationTypeService _locationTypeService;

        public LocationTypeController()
        {
            this._locationTypeService = new LocationTypeService();
        }

        public ActionResult Index()
        {
            return View("Index", LocationTypeViewModel.ToViewModel(new LocationType()));
        }

        public ActionResult Edit(int id)
        {
            return View("Edit", LocationTypeViewModel.ToViewModel(this._locationTypeService.FindOne(id)));
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult Edit_POST(int id, LocationTypeViewModel locationTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    LocationType locationType = this._locationTypeService.FindOne(id);
                    if (locationType != null)
                    {
                        this._locationTypeService.Update(locationType, locationTypeViewModel.LocationType);
                        var message = String.Format("Platstypen {0} är nu redigerad", locationType.location_type_swe);
                        ViewData["message"] = MvcHtmlString.Create(String.Format("<a href='{0}'>{1}</a>", Url.Action("Edit", "LocationType", new RouteValueDictionary(new { id = locationType.location_type_id })), message));
                                
                        return View("Index", LocationTypeViewModel.ToViewModel(new LocationType()));
                    }
                    return View("NotFound");

                }
                catch (Exception e)
                {
                    ModelState.AddModelError(String.Empty, e.Message);
                }
            }
            return View("Edit", locationTypeViewModel);
        }

        public ActionResult Create()
        {
            var a = LocationTypeViewModel.ToViewModel(new LocationType());
            return View("Create", LocationTypeViewModel.ToViewModel(new LocationType()));
        }

        [HttpPost]
        [ActionName("Create")]
        public ActionResult Create_POST(LocationTypeViewModel locationTypeViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var location_type_id = this._locationTypeService.Add(locationTypeViewModel.LocationType);
                    var message = String.Format("Platstypen {0} är nu skapad", locationTypeViewModel.LocationType.location_type_swe);
                    ViewData["message"] = MvcHtmlString.Create(String.Format("<a href='{0}'>{1}</a>", Url.Action("Edit", "LocationType", new RouteValueDictionary(new { id = location_type_id })), message));
                    return View("Index", LocationTypeViewModel.ToViewModel(new LocationType()));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(String.Empty, e.Message);
                }
            }
            return View("Create", locationTypeViewModel);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                LocationType locationType = this._locationTypeService.FindOne(id);
                if(locationType == null)
                {
                    return View("NotFound");
                }
                return View("Delete", locationType);
            }
            catch(Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
            }
            return View("Delete");
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult Delete_POST(int id)
        {
            try
            {
                LocationType locationType = this._locationTypeService.FindOne(id);
                if (locationType != null)
                {
                    try
                    {
                        string name = locationType.location_type_swe;
                        this._locationTypeService.Delete(locationType);
                        ViewData["message"] = String.Format("Platstypen {0} är nu Borttagen", name);
                        return View("Index", LocationTypeViewModel.ToViewModel(new LocationType()));
                    }
                    catch
                    {
                        return View("DeleteError");
                    }
                }
                return View("NotFound");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
                return View("ConnectionError");
            }
        }
    }
}
