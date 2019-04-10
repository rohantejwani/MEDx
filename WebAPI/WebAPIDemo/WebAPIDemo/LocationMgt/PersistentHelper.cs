using System;
using System.Collections.Generic;
using DTO;
using PersistenceManager;
using Utility;
using Newtonsoft.Json;
using System.ServiceModel.Web;
using log4net.Core;

namespace WebAPIDemo.LocationMgt
{
    internal class PersistentHelper
    {
        ErrorConstants ec = new ErrorConstants();
        public PersistentHelper()
        {
        }

        internal List<Location> GetAllLocation()
        {
            Logger.Log(Level.Info, "Enter Get All Locations");
            List<Location> locations = new List<Location>();
            List<List<Dictionary<String, string>>> persistentCarrier = new List<List<Dictionary<String, string>>>();
            try
            {
                String query = "SELECT * FROM mlo.location where updated = 'YES';";
                persistentCarrier = DBUtility.ExecuteQuery(query);

                SBMapper map = new SBMapper();
                map.MapperCollection = PropertyMapper.MapLocation();
                Location loc = null;
                foreach (Dictionary<string, string> eachCarrier in persistentCarrier[0])
                {
                    loc = new Location();
                    string json =new DTOMapper().Mapper(eachCarrier, loc, map);
                    loc = JsonConvert.DeserializeObject<Location>(json);
                    locations.Add(loc);
                }
        }
            catch (Exception ex)
            {
                Logger.Log(Level.Error, "Error in Get All Locations :: " + ex.Message + "\n Caused By :- " + ex.StackTrace);
                throw ex;
            }
            return locations;
        }

        internal string RegisterLocation(Location location)
        {
            String query = "INSERT INTO `mlo`.`location` (name, latitude, longitude, address) " +
                "VALUES ('" + location.name + "','" + location.latitude + "','" + location.longitude + "','" + location.address + "'); ";
            try
            {
                DBUtility.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                //Logger.Log(Level.Error, "Error in Sign Up :: " + ex.Message + "\n Caused By :- " + ex.StackTrace);
                //throw new WebFaultException<CustomFault>(new CustomFault("Error in Sign Up ", skyboInternalSvrErr, ex.Message), internalSvrErr);
                throw ex;
            }
            return "Success";
        }
    }
}