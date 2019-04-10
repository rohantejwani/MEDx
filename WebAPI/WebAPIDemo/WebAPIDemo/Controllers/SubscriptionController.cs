using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPIDemo.Controllers
{
    public class SubscriptionController : ApiController
    {
        public List<Subscriptions> GetAllSubscription()
        {
            List<Subscriptions> subscriptions = null;
            try
            {
                subscriptions = new SubsMgt.PersistentHelper().GetAllSubscription();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return subscriptions;
        }

        [HttpPost]
        public string AddNewSubscription([FromBody]List<Subscriptions> subscriptions)
        {
            return new SubsMgt.PersistentHelper().AddNewSubscription(subscriptions);
        }
    }
}
