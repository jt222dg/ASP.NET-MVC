using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using MapAdminInterface.Models;

namespace MapAdminInterface.ViewModels
{
    public class LocationTypeViewModel
    {
        private MainService _mainService;

        public LocationTypeViewModel()
            :this(null)
        {
        
        }

        public LocationTypeViewModel(LocationType locationType)
        {
            this._mainService = new MainService();
            this.LocationTypes = this._mainService.GetLocationTypes();
            this.Icons = this._mainService.GetIcons();
            this.LocationType = locationType;
        }

        public static LocationTypeViewModel ToViewModel(LocationType locationType)
        {
            return new LocationTypeViewModel(locationType);
        }

        public static List<LocationTypeViewModel> ToViewModels(IEnumerable<LocationType> locationTypes)
        {
            List<LocationTypeViewModel> ltvms = new List<LocationTypeViewModel>();

            foreach (LocationType locationType in locationTypes)
                ltvms.Add(new LocationTypeViewModel(locationType));

            return ltvms;
        }

        public LocationType LocationType { get; set; }
        public List<LocationType> LocationTypes { get; set; }

        public List<Icon> Icons { get; set; }
    }
}