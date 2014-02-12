using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MapAPI.ViewModels;

namespace MapAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            HomeIndexViewModel hivm = new HomeIndexViewModel();
            hivm.GenerateList();

            return View("Index", hivm);
        }
    }
}
