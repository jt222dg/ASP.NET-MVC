using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MapAdminInterface.Models
{
    public class LocationTypeService : AbstractService
    {
        public void Delete(LocationType locationtype)
        {
            try
            {
                this._repository.Delete(locationtype);
                this.Save();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Add(LocationType locationtype)
        {
            try
            {
                this._repository.Add<LocationType>(locationtype);
                this.Save();
                return locationtype.location_type_id;
            }
            catch (Exception)
            {
                throw new Exception("Kunde inte lägga till platstypen(säkerställ att ingen platstyp med namnet redan finns.)");
            }
        }

        public LocationType FindOne(int id)
        {
            try
            {
                return this._repository.Find<LocationType>(lt => lt.location_type_id == id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<LocationType> FindAll()
        {
            try
            {
                return this._repository.FindAll<LocationType>();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void Update(LocationType oldLocationType, LocationType newLocationType)
        {
            try
            {
                oldLocationType.location_type_swe = newLocationType.location_type_swe;
                oldLocationType.location_type_eng = newLocationType.location_type_eng;
                oldLocationType.icon_id = newLocationType.icon_id;

                this.Save();
            }
            catch (Exception)
            {
                throw new Exception("Kunde inte spara platsTypen");
            }
        }
    }
}