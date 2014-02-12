using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MapUserInterface.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["lang"] = "sv";

            return View();
        }

        public ActionResult En() {
            ViewData["lang"] = "en";

            return View();
        }   
    }
}
