using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MapAdminInterface.Models;

namespace MapAdminInterface.ViewModels
{
    public class LocationViewModel
    {
        private MainService _mainService;

        public LocationViewModel()
            : this(null)
        {
        
        }

        public LocationViewModel(Location location)
        {
            this._mainService = new MainService();

            this.SearchResult = null;
            this.SearchTerm = "";
            this.AreaTerm = "";
            this.Location = location;

            this.Cities = this._mainService.GetCities();

            this.Areas = this._mainService.GetAreas();
            this.LocationTypes = this._mainService.GetLocationTypes();
 
            this.FloorNumbers = new List<int>();
            for (int i = -1; i < 10; ++i)
                FloorNumbers.Add(i + 1);

            this.HasChosenCity = false;
            this.HasChosenArea = false;
            this.TriedToAddPlace = false;
        }

        public static object ToViewModel(Location location)
        {
            return new LocationViewModel(location);
        }

        public bool HasSearchResults()
        {
            return (this.SearchResult != null && this.SearchResult.Count() > 0);
        }

        public bool HasMadeSearch()
        {
            return (this.SearchResult != null);
        }

        public void AddLocationNames()
        {
            this.Location.LocationNames.Add(new LocationName());
        }

        public IEnumerable<Area> CityAreas(int id)
        {
            try
            {
                var a = this._mainService.GetCitiesByAreaId(id);
                return a;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public string GetCityName(int id)
        {
            try
            {
                var a = this._mainService.GetAreaById(id);
                this.Location.Area = a;
                return a.City.city1;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Area GetArea(int area_id)
        {
            return this._mainService.GetAreaById(area_id);
        }
        
        public Location Location { get; set; }
        public IEnumerable<City> Cities { get; set; }
        public IEnumerable<Area> Areas { get; set; }
        
        public IEnumerable<LocationType> LocationTypes { get; set; }
        public List<int> FloorNumbers { get; set; }

        public int City_id { get; set; }

        public IEnumerable<Location> SearchResult { get; set; }

        [DisplayName("Sök efter platens namn")]
        public string SearchTerm { get; set; }

        public string AreaTerm { get; set; }

        public bool HasChosenCity { get; set; }
        public bool HasChosenArea { get; set; }
        public bool TriedToAddPlace { get; set; }
    }
}