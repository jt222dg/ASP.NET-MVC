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
    public class LocationController : Controller
    {
        private LocationService _locationService;
        private MainService _mainService;

        public LocationController()
        {
            this._locationService = new LocationService();
            this._mainService = new MainService();
        }

        public ActionResult Index()
        {
            return View("Index", LocationViewModel.ToViewModel(new Location()));
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index_POST([Bind(Include="SearchTerm")]LocationViewModel locationViewModel)
        {
            if (ModelState.IsValid)
            {
                locationViewModel.SearchResult = this._locationService.FindBySearch(locationViewModel.SearchTerm);
            }
            return View("Index", locationViewModel);
        }

        public ActionResult ShowAreaLocations(int id)
        {
            LocationViewModel locationViewModel = new LocationViewModel();
            AreaService _areaService = new AreaService();
            locationViewModel.AreaTerm = _areaService.FindOne(id).area_swe;
            locationViewModel.SearchResult = this._locationService.LocationsByArea(id);
            return View("Index", locationViewModel);
        }

        public ActionResult Create()
        {
            return View("Create", LocationViewModel.ToViewModel(new Location()));
        }

        [HttpPost]
        [ActionName("Create")]
        public ActionResult Create_POST(LocationViewModel locationViewModel, string save)
        {
            if (!String.IsNullOrEmpty(save))
            {
                //strip all deleted names
                locationViewModel.Location.LocationNames.Where(ln => ln.Delete == true).ToList().ForEach(lnd => locationViewModel.Location.LocationNames.Remove(lnd));
                if (locationViewModel.Location.LocationNames.Count() > 0)
                {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            foreach (LocationName name in locationViewModel.Location.LocationNames)
                            {
                                try
                                {
                                    this._mainService.NameAlreadyExists(name.location_name_swe, "swedish");
                                }
                                catch (Exception e)
                                {

                                    ModelState.AddModelError( String.Empty, e.Message);
                                    return View("Create", locationViewModel);
                                }
                                try
                                {
                                    this._mainService.NameAlreadyExists(name.location_name_eng, "english");
                                }
                                catch (Exception e)
                                {
                                    ModelState.AddModelError(String.Empty, e.Message);
                                    return View("Create", locationViewModel);
                                }
                            }

                            var location_id = this._locationService.Add(locationViewModel.Location);
                            var message = String.Format("Platsen {0} är nu skapad", locationViewModel.Location.LocationNames.Where(ln => ln.is_main).First().location_name_swe);
                            ViewData["message"] = MvcHtmlString.Create(String.Format("<a href='{0}'>{1}</a>", Url.Action("Edit", "Location", new RouteValueDictionary(new { id = location_id })), message));
                            
                            return View("Index", LocationViewModel.ToViewModel(new Location()));
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError(String.Empty, e.Message);
                        }
                    }
                    else
                    {
                        TryUpdateModel(locationViewModel);
                    }
                }
                else
                {
                    TryUpdateModel(locationViewModel);
                    ModelState.AddModelError(String.Empty, "Du måste ange minst ett namn");
                }
            }
            else
            {
                ModelState.Clear();
            }
            return View("Create", locationViewModel);
        }

        public ActionResult Edit(int id)
        {
            return View("Edit", LocationViewModel.ToViewModel(this._locationService.FindOne(id)));
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult Edit_POST(int id, LocationViewModel locationViewModel, string save)
        {
            if (!String.IsNullOrEmpty(save))
            {
                //strip all deleted names
                locationViewModel.Location.LocationNames.Where(ln => ln.Delete == true).ToList().ForEach(lnd => locationViewModel.Location.LocationNames.Remove(lnd));
                if (locationViewModel.Location.LocationNames.Count() > 0)
                {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            Location location = this._locationService.FindOne(id);
                            if (location != null)
                            {
                                foreach (LocationName name in locationViewModel.Location.LocationNames)
                                {
                                    try
                                    {
                                        this._mainService.NameAlreadyExists(name.location_name_swe, "swedish", location.LocationNames.ToList());
                                    }
                                    catch (Exception e)
                                    {
                                        ModelState.AddModelError(String.Empty, e.Message);
                                        return View("Edit", locationViewModel);
                                    }
                                    try
                                    {
                                        this._mainService.NameAlreadyExists(name.location_name_eng, "english", location.LocationNames.ToList());
                                    }
                                    catch (Exception e)
                                    {
                                        ModelState.AddModelError(String.Empty, e.Message);
                                        return View("Edit", locationViewModel);
                                    }
                                }

                                this._locationService.Update(location, locationViewModel.Location);
                                var message = String.Format("Platsen {0} är nu redigerad", location.LocationNames.Where(ln => ln.is_main).First().location_name_swe);
                                ViewData["message"] = MvcHtmlString.Create(String.Format("<a href='{0}'>{1}</a>", Url.Action("Edit", "Location", new RouteValueDictionary(new { id = location.location_id })), message));
                                
                                return View("Index", LocationViewModel.ToViewModel(new Location()));
                            }
                            return View("NotFound");
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError(String.Empty, e.Message);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Du måste ange minst ett namn");
                }
            }
            else
            {
                ModelState.Clear();
            }
            TryUpdateModel(locationViewModel);
            return View("Edit", locationViewModel);
        }

        public ActionResult Delete(int id)
        {
            Location location = this._locationService.FindOne(id);
            if (location == null)
            {
                return View("NotFound");
            }
            return View("Delete", location);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult Delete_POST(int id)
        {
            try
            {
                Location location = this._locationService.FindOne(id);
                if (location != null)
                {
                    string name = location.LocationNames.Where(ln => ln.is_main).First().location_name_swe;
                    this._locationService.Delete(location);
                    ViewData["message"] = String.Format("Platsen {0} är nu borttagen", name);
                    return View("Index", LocationViewModel.ToViewModel(new Location()));
                }
                return View("NotFound");
            }
            catch (Exception)
            {
                return View("ConnectionError");
            }
        }
    }
}
