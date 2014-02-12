using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MapAdminInterface.Models;

namespace MapAdminInterface.Controllers
{
    public class MainController : Controller
    {
        public MainController()
        {
        }

        public ActionResult Index()
        {
            return View("Index");
        }
        public ActionResult About()
        {
            return View("About");
        }

    }
}
