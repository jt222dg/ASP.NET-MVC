using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MapAdminInterface.Models
{
    public class LocationService : AbstractService
    {
        private MapEntities _entities;

        public LocationService()
            : base()
        {
            this._entities = new MapEntities();
        }

        public void Delete(Location location)
        {
            this._repository.Delete(location);

            this.Save();
        }

        public int Add(Location location)
        {
            try
            {
                Location loc = new Location();
                loc.area_id = location.area_id;
                loc.floor_nr = location.floor_nr;
                loc.latitude = location.latitude;
                loc.longitude = location.longitude;
                loc.location_type_id = location.location_type_id;
                this._repository.Add(loc);

                this.Save();

                var location_id = loc.location_id;

                //TODO remove usp-create-location

                //add new names
                this.AddNames(location.LocationNames, location_id);

                this.Save();

                return location_id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void AddNames(IEnumerable<LocationName> locationNames, int id)
        {
            try
            {
                //Set the main name
                locationNames.ToList().First().is_main = true;
                // Set the user id (foreign key) for the tweets.
                locationNames.ToList().ForEach(ln => ln.location_id = id);
                //Set default english name
                locationNames.Where(ln => ln.location_name_eng == null).ToList().ForEach(ln => ln.location_name_eng = ln.location_name_swe);
                //Add the names
                locationNames.ToList().ForEach(ln => this._repository.Add<LocationName>(ln));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void RemoveNames(IEnumerable<LocationName> oldLocationNames)
        {
            try
            {
                oldLocationNames.ToList().ForEach(oln => this._repository.Delete<LocationName>(oln));
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Location FindOne(int id)
        {
            try
            {
                return this._repository.Find<Location>(l => l.location_id == id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<Location> FindAll()
        {
            try
            {
                return this._repository.FindAll<Location>();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<Location> FindBySearch(string searchTerm)
        {
            try
            {
                var locs = this._entities.usp_search_locations(searchTerm).ToList();
                return locs;
            }
            catch (Exception)
            {
                throw new Exception("Det gick inte att ansluta mot databasen, försök igen senare eller kontakta personal.");
            }
        }
        public void Update(Location oldLocation, Location newLocation)
        {
            try
            {
                oldLocation.location_type_id = newLocation.location_type_id;
                oldLocation.area_id = newLocation.area_id;
                oldLocation.latitude = newLocation.latitude;
                oldLocation.longitude = newLocation.longitude;
                oldLocation.floor_nr = newLocation.floor_nr;

                this.RemoveNames(oldLocation.LocationNames);
                this.AddNames(newLocation.LocationNames, oldLocation.location_id);

                this.Save();
            }
            catch (Exception)
            {
                throw new Exception("Kunde inte spara platsen.");
            }
        }
        public List<Location> LocationsByArea(int id)
        {
            return this._entities.usp_area_locations(id).ToList();
        }
    }
}