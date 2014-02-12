using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using MapAPI.Models;

namespace MapAPI.ViewModels
{
    public class HomeIndexViewModel
    {
        private IMapRepository _repository;
        private int _building;
        private double _latitude;
        private double _longitude;
        private string _name;
        private string _translation;
        private string _city;
        private int _floor;

        public string Path
        {
            private set {}
            get
            {
                return HttpContext.Current.Server.MapPath("~/Mobile/List.txt");
            }
        }

        public HomeIndexViewModel()
        {
            this._repository = new MapRepository();
            this._building = 0;
            this._latitude = 0.0;
            this._longitude = 0.0;
            this._name = "";
            this._translation = "";
            this._city = "";
            this._floor = 1;
        }

        public void GenerateList()
        {
            using (var file = File.Create(Path))
            {
                using (StreamWriter sw = new StreamWriter(file, Encoding.UTF8))
                {
                    List<MobileLocation> locations = MobileLocation.ToMobiles(
                        this._repository.GetAllLocationsWithAreaAndCity()).ToList();

                    foreach (MobileLocation location in locations)
                    {
                        string line = "0;";
                        
                        line += location.Location.Latitude.ToString() + ";";
                        line += location.Location.Longitude.ToString() + "|";

                        foreach (Name name in location.Location.Names)
                        {
                            line += name.Swedish + ", ";
                            line += location.AreaName.Swedish;
                            if (location.AreaName.English != "")
                                line += " (" + location.AreaName.English + "), ";

                            line += location.CityName + ";";
                        }
                        line = line.Remove(line.Length - 1);

                        line += "|Våning " + location.Location.FloorNumber.ToString();
                        if (location.Location.Names.Count() > 0)
                            line += ", rum " + location.Location.Names.First().Swedish;
                        
                        line += ". ";
                        line += location.LocationTypeName.Swedish;
                        line += ".;sv_SE;";

                        line += "Floor " + location.Location.FloorNumber.ToString();
                        if (location.Location.Names.Count() > 0)
                            line += ", room " + location.Location.Names.First().Swedish;

                        line += ". ";
                        line += location.LocationTypeName.English;
                        line += ".;en_US;";
                        sw.WriteLine(line);
                    }

                    List<MobileArea> areas = MobileArea.ToMobiles(this._repository.GetAll<Area>()).ToList();

                    foreach (MobileArea area in areas)
                    {
                        string line = "1;";
                        line += area.Area.Latitude.ToString() + ";";
                        line += area.Area.Longitude.ToString() + "|";
                        line += area.Area.Name.Swedish + ", ";

                        if (area.Area.Name.English != "")
                            line += "(" + area.Area.Name.English + "), ";
                        
                        line += area.CityName + "|Byggnad;sv_SE;Building;en_US";

                        sw.WriteLine(line);
                    }
                }
            }
        }
    }
}