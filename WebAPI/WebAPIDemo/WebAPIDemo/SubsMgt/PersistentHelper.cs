using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO;
using Newtonsoft.Json;
using PersistenceManager;
using Utility;

namespace WebAPIDemo.SubsMgt
{
    public class PersistentHelper
    {
        internal List<Subscriptions> GetAllSubscription()
        {
            List<List<Dictionary<string, string>>> persistentCarrier = new List<List<Dictionary<string, string>>>();
            string query = "Select * from mlo.subscriptions;";
          
            Subscriptions subs = null;
            List<Subscriptions> subsList = new List<Subscriptions>();
            try
            {
                persistentCarrier = DBUtility.ExecuteQuery(query);
                SBMapper map = new SBMapper(PropertyMapper.MapSubscriptions());
                foreach (Dictionary<string, string> eachCarrier in persistentCarrier[0])
                {
                    subs = new Subscriptions();
                    string json = new DTOMapper().Mapper(eachCarrier, subs, map);
                    subs = JsonConvert.DeserializeObject<Subscriptions>(json);
                    subsList.Add(subs);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return subsList;
        }

        internal string AddNewSubscription(List<Subscriptions> subscriptions)
        {
            //String query = "INSERT INTO `mlo`.`location` (name, latitude, longitude, address) " +
            //    "VALUES ('" + location.name + "','" + location.latitude + "','" + location.longitude + "','" + location.address + "'); ";
            //try
            //{
            //    DBUtility.ExecuteQuery(query);
            //}
            //catch (Exception ex)
            //{
            //    //Logger.Log(Level.Error, "Error in Sign Up :: " + ex.Message + "\n Caused By :- " + ex.StackTrace);
            //    //throw new WebFaultException<CustomFault>(new CustomFault("Error in Sign Up ", skyboInternalSvrErr, ex.Message), internalSvrErr);
            //    throw ex;
            //}
            return "Success";
        }
    }
    
}