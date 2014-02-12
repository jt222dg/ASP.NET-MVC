using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MapAdminInterface.Models
{
    public class AreaService : AbstractService
    {
        private MainService _mainService;

        public AreaService()
        {
            this._mainService = new MainService();
        }

        public Area FindOne(int id)
        {
            try
            {
                return this._repository.Find<Area>(a => a.area_id == id);
            }
            catch (Exception)
            {
                throw new Exception("Kunde inte hitta området.");
            }
        }

        public IEnumerable<Area> FindAll()
        {
            try
            {
                return this._repository.FindAll<Area>();
            }
            catch (Exception)
            {
                throw new Exception("Kan inte ansluta till databasen, försök igen senare eller kontakta personal.");
            }
        }

        public void Delete(Area area)
        {
            try
            {
                this._repository.Delete<Area>(area);
                this.Save();
            }
            catch (Exception)
            {
                throw new Exception("Kunde inte ta bort området. (Säkerställ att inga platser är kopplade till det och försök igen.)");
            }
        }

        public int Add(Area area)
        {
            try
            {
                if (String.IsNullOrEmpty(area.area_eng))
                    area.area_eng = area.area_swe;

                this._repository.Add<Area>(area);
                this.Save();
                return area.area_id;
            }
            catch (Exception)
            {
                throw new Exception("Kunde inte lägga till området. (Säkerställ att inget område med samma namn finns)");
            }
        }

        public void Update(Area oldArea, Area newArea)
        {
            oldArea.area_swe = newArea.area_swe;
            oldArea.area_eng = newArea.area_eng;
            if (String.IsNullOrEmpty(oldArea.area_eng))
                oldArea.area_eng = oldArea.area_swe;

            oldArea.City = newArea.City;
            oldArea.city_id = newArea.city_id;
            oldArea.latitude = newArea.latitude;
            oldArea.longitude = newArea.longitude;

            try
            {
                this.Save();
            }
            catch (Exception)
            {
                throw new Exception("Kunde inte spara området, säkerställ att angivet namn är tillgängligt!");
            }
        }
    }
}