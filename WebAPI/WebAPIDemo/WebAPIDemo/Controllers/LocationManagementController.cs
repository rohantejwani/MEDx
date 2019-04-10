using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utility;

namespace WebAPIDemo.Controllers
{
    public class LocationManagementController : ApiController
    {
        //CustExceptn ec = new CustExceptn();
        public List<Location> GetAllLocation()
        {
            List<Location> locations = null;
            try
            {
                locations = new LocationMgt.PersistentHelper().GetAllLocation();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return locations;
        }

        [HttpPost]
        public string RegisterLocation([FromBody]Location location)
        {
            return new LocationMgt.PersistentHelper().RegisterLocation(location);
        }
    }
}
