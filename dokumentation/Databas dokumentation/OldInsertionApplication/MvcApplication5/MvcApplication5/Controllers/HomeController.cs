using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication5.Models;

namespace MvcApplication5.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            try
            {
                Service serv = new Service();
                List<Location> list = serv.GetAllLocations();

                List<Area> arList = new List<Area>();
                List<Location> locList = new List<Location>();

                foreach (var row in list)
                {
                    if (row.typ == 1)
                    {
                        Area ar = new Area();
                        ar.typ = row.typ;
                        ar.latitud = row.latitud;
                        ar.longitud = row.longitud;
                        ar.stad = row.stad;
                        ar.byggnad_sv = row.byggnad_sv;
                        ar.byggnad_en = row.byggnad_en;
                        arList.Add(ar);
                    }
                    else
                    {
                        if (row.vaning < 0)
                        {
                            row.vaning = 0;
                        }
                        locList.Add(row);
                    }
                }
                ////Sjöfartshögskolan fattas som area
                //Area missingAr1 = new Area();
                //missingAr1.typ = 1;
                //missingAr1.latitud = null;
                //missingAr1.longitud = null;
                //missingAr1.stad = "Kalmar";
                //missingAr1.byggnad_sv = "Sjöfartshögskolan";
                //missingAr1.byggnad_en = "null";
                //arList.Add(missingAr1);

                //Area missingAr2 = new Area();
                //missingAr2.typ = 1;
                //missingAr2.latitud = null;
                //missingAr2.longitud = null;
                //missingAr2.stad = "Kalmar";
                //missingAr2.byggnad_sv = "Kocken";
                //missingAr2.byggnad_en = "null";
                //arList.Add(missingAr2);

                //Area missingAr3 = new Area();
                //missingAr3.typ = 1;
                //missingAr3.latitud = null;
                //missingAr3.longitud = null;
                //missingAr3.stad = "Kalmar";
                //missingAr3.byggnad_sv = "Smålandsgatan";
                //missingAr3.byggnad_en = "null";
                //arList.Add(missingAr3);

                serv.InsertAreas(arList);
                serv.InsertLocations(locList);
            }
            catch (Exception ex)
            {
                //Fångar upp eventuella databasfel.
                var error = ex.InnerException.Message.ToString();
                ModelState.AddModelError(String.Empty, error);
            }
            return View();
        }

    }
}
