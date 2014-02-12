using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace MvcApplication5.Models
{
    public class Service
    {
        private kartaEntities _entities = new kartaEntities();

        public List<Location> GetAllLocations()
        {
            try
            {
                XDocument doc;

                var requestUriString = "http://localhost:3616/Content/mapdata.xml";

                var request = WebRequest.Create(requestUriString);

                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    doc = XDocument.Load(stream);
                }
                return (from item in doc.Descendants("Row").Skip(1)
                        select LocationFactory.Create(item)).ToList();


            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void InsertLocations(List<Location> list)
        {
                list
                  .ForEach(t => this._entities.usp_insert_OldDATA(
                          t.latitud,
                          t.longitud,
                          t.ritningsnamn,
                          t.rumsnamn2,
                          t.rumsnamn3,
                          t.rumsnamn4,
                          t.byggnad_sv,
                          t.byggnad_en,
                          t.vaning,
                          t.stad,
                          t.rumstyp_sv,
                          t.rumstyp_en
                      ));
        }

        public void InsertAreas(List<Area> list)
        {
            list
              .ForEach(t => this._entities.usp_Area(
                      t.stad,
                      t.latitud,
                      t.longitud,
                      t.byggnad_sv,
                      t.byggnad_en
                  ));

        }
    }
}