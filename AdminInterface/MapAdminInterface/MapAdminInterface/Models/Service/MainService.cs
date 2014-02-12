using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MapAdminInterface.Models
{
    public class MainService : AbstractService
    {
        List<LocationName> names;

        public MainService()
        {
            names = this._repository.FindAll<LocationName>().ToList();
        }

        public IEnumerable<City> GetCities()
        {
            try
            {
                return this._repository.FindAll<City>();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<Area> GetAreas()
        {
            try
            {
                return this._repository.FindAll<Area>().ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public Area GetAreaById(int id)
        {
            try
            {
                return this._repository.Find<Area>(a => a.area_id == id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public City GetCityById(int id)
        {
            try
            {
                return this._repository.Find<City>(c => c.city_id == id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<LocationType> GetLocationTypes()
        {
            try
            {
                return this._repository.FindAll<LocationType>().ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public List<Icon> GetIcons()
        {
            try
            {
                return this._repository.FindAll<Icon>().ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<Area> GetCitiesByAreaId(int id)
        {
            return this._repository.Query<Area>()
                .Where(a => a.city_id == id);
        }

        public void NameAlreadyExists(string str, string language, List<LocationName> oldNames = null)
        {
            if (oldNames != null)
            {
                switch (language)
                {
                    case "swedish":
                        foreach (LocationName oldName in oldNames)
                            if (oldName.location_name_swe == str)
                                return;
                        break;
                    case "english":
                        foreach (LocationName oldName in oldNames)
                            if (oldName.location_name_eng == str)
                                return;
                        break;
                    default:
                        break;
                }
            }
            switch (language)
            {
                case "swedish":
                    foreach (LocationName name in names)
                        if (str == name.location_name_swe)
                        {
                            throw new Exception(String.Format("Det angivna namnet {0} är upptaget!", str));
                        }
                    break;
                case "english":
                    foreach (LocationName name in names)
                        if (str == name.location_name_eng)
                        {
                            throw new Exception(String.Format("Den angivna översättningen {0} är upptagen!", str));
                        }
                    break;
                default:
                    break;
            }

            return;
        }
    }
}