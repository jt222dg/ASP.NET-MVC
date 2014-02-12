using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MapAdminInterface.Models;

namespace MapAdminInterface.ViewModels
{
    public class AreaViewModel
    {
        private MainService _mainService;

        public AreaViewModel()
            : this(null)
        {
        
        }

        public AreaViewModel(Area area)
        {
            this._mainService = new MainService();
            this.Area = area;
            this.Cities = this._mainService.GetCities();
            this.Areas = this._mainService.GetAreas();
            this.HasChosenCity = false;
        }

        public static AreaViewModel ToViewModel(Area area)
        {
            return new AreaViewModel(area);
        }

        public static List<AreaViewModel> ToViewModels(IEnumerable<Area> areas)
        {
            List<AreaViewModel> avms = new List<AreaViewModel>();

            foreach (Area area in areas)
                avms.Add(new AreaViewModel(area));

            return avms;
        }
        public City GetCity(int city_id)
        {
            return this._mainService.GetCityById(city_id);
        }

        public Area Area { get; set; }
        public IEnumerable<Area> Areas { get; set; }
        public IEnumerable<City> Cities { get; set; }
        public bool HasChosenCity { get; set; }
    }
}