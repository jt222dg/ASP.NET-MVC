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
    public class AreaController : Controller
    {
        private AreaService _areaService;

        public AreaController()
        {
            this._areaService = new AreaService();
        }

        public ActionResult Index()
        {
            try
            {
                return View("Index", AreaViewModel.ToViewModel(new Area()));
            }
            catch (Exception)
            {
                return View("ConnectionError");
            }
        }

        public ActionResult Create()
        {
            return View("Create", AreaViewModel.ToViewModel(new Area()));
        }

        [HttpPost]
        [ActionName("Create")]
        public ActionResult Create_POST(AreaViewModel areaViewModel, string save)
        {
            if (!String.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var area_id = this._areaService.Add(areaViewModel.Area);
                        var message = String.Format("Området {0} är nu skapat", areaViewModel.Area.area_swe);
                        ViewData["message"] = MvcHtmlString.Create(String.Format("<a href='{0}'>{1}</a>",Url.Action("Edit", "Area", new RouteValueDictionary(new { id = area_id })), message));
                        return View("Index", AreaViewModel.ToViewModel(new Area()));
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError(String.Empty, e.Message);
                    }
                }
            }
            else
            {
                ModelState.Clear();
            }
         
            return View("Create", areaViewModel);
        }

        public ActionResult Edit(int id)
        {
            AreaViewModel avm = null;

            try
            {
                avm = new AreaViewModel(this._areaService.FindOne(id));
                if (avm.Area != null)
                {
                    return View("Edit", avm);
                }
                return View("NotFound");
            }
            catch (Exception)
            {
                return View("ConnectionError");
            }
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult Edit_POST(int id, AreaViewModel areaViewModel, string save)
        {
            if (!String.IsNullOrEmpty(save))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Area area = this._areaService.FindOne(id);
                        if (area == null)
                        {
                            return View("NotFound");
                        }
                        try
                        {
                            this._areaService.Update(area, areaViewModel.Area);
                            var message = String.Format("Området {0} är nu redigerat", areaViewModel.Area.area_swe);
                            ViewData["message"] = MvcHtmlString.Create(String.Format("<a href='{0}'>{1}</a>", Url.Action("Edit", "Area", new RouteValueDictionary(new { id = area.area_id })), message));

                            return View("Index", AreaViewModel.ToViewModel(new Area()));
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError(String.Empty, e.Message);
                        }
                    }
                    catch (Exception)
                    {
                        return View("ConnectionError");
                    }
                }
                else if (areaViewModel.HasChosenCity == true)
                {
                    ModelState.Clear();
                }
            }

            return View("Edit", areaViewModel);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                Area area = this._areaService.FindOne(id);
                if (area == null)
                {
                    return View("NotFound");
                }
                return View("Delete", AreaViewModel.ToViewModel(area));
            }
            catch (Exception)
            {
                return View("ConnectionError");
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult Delete_POST(int id)
        {
            try
            {
                Area area = this._areaService.FindOne(id);
                if (area != null)
                {
                    try
                    {
                        string name = area.area_swe;
                        this._areaService.Delete(area);
                        ViewData["message"] = String.Format("Området {0} är nu borttaget", name);
                        return View("Index", AreaViewModel.ToViewModel(new Area()));
                    }
                    catch
                    {
                        return View("DeleteError");
                    }
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
