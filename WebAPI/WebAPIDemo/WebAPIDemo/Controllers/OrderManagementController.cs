using DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PersistenceManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Utility;
using Newtonsoft.Json.Linq;
using WebAPIDemo.OrderMgt;
using System.Web.Http.Cors;
using log4net.Core;

namespace WebAPIDemo.Controllers
{


    public class OrderManagementController : ApiController
    {
        public string GetTest()
        {
            return "Service is up";
        }


        [HttpPost]
       // [EnableCors(origins: "http://localhost:50158", headers: "*", methods: "*")]
        public string CreateOrder([FromBody]OrderDetails orderDetails)
        {
            if (orderDetails != null)
            {
                //var jsonString = json.ToString();
                //OrderDetails orderDetails = JsonConvert.DeserializeObject<OrderDetails>(jsonString);

                if (string.IsNullOrEmpty(orderDetails.destination) || string.IsNullOrEmpty(orderDetails.challanNumber))
                    throw new ArgumentException("Destination or challan number cannot be null/empty");
                if (orderDetails.requestTime == null)
                    throw new ArgumentException("Request time cannot be null");
                if (orderDetails.noOfItems <= 0)
                    throw new ArgumentException("Number of items not specified");


                //if order exists with the same retailer name and challan number then donot create order 
                OrderDetails order = new PersistentHelper().GetSpecificOrder(orderDetails.destination, orderDetails.challanNumber);
                if (order == null)
                    return new PersistentHelper().CreateOrder(orderDetails);
                else
                    return "Order already exist";
            }
            else
            {
                throw new Exception("Input is null.");
            }
        }
        //unimplemented
        public List<OrderDetails> GetAcceptedOrder(string id)
        {
            throw new NotImplementedException();
        }

        public List<OrderDetails> GetOrderForEmployee(string empType, string id, string state = "")
        {
            if (string.IsNullOrEmpty(empType) || string.IsNullOrEmpty(id))
                throw new ArgumentException("Employee type or id cannot be null/empty");

            return new PersistentHelper().GetOrderForEmployee(empType, id, state);
        }

        public List<OrderDetails> GetRouteOrders(string driverId, string id = ""  )
        {
            if ( string.IsNullOrEmpty(driverId))
                throw new ArgumentException("Driver id cannot be null/empty");

            return new PersistentHelper().GetRouteOrders( id, driverId);
        }

        public List<OrderDetails> GetOrderForWarehouse(string userId="", string state="")
        {
            return new PersistentHelper().GetOrderForWarehouse(userId, state);
        }
        public List<OrderDetails> GetAllOrders(string userId = "", string state = "")
        {
            return new PersistentHelper().GetAllOrders(userId, state);
        }
        
        public List<OrderDetails> GetOrderForUser(string userId = "", string state = "")
        {
            Logger.Log(Level.Debug,"Enter GetOrderForUser");
            return new PersistentHelper().GetOrderForUser(userId, state);
        }


        [HttpPost]

        //input modified (id - userId)
        public string AcceptOrder([FromBody] object json)
        {
            if (json != null)
            {
                JObject jObject = JObject.Parse(json.ToString());
                string id = (string)jObject["id"];
                string orderId = (string)jObject["orderId"];

                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(orderId))
                    throw new Exception("Id and orderid cannot be null/empty.");

                if (jObject["requestTime"] == null)
                    throw new Exception("Request Time cannot be null/empty.");

                string requestTime = Convert.ToDateTime(jObject["requestTime"]).ToString("yyyy-MM-dd HH:mm:ss");

                return new PersistentHelper().AcceptOrder(id, orderId, requestTime);
            }
            else
            {
                throw new Exception("Input is null.");
            }
        }

        [HttpPost]

        // API rename DeliveredOrder - OrderDeleivered
        // API accepts id as input but does not use while updating the status
        public string OrderDelivered([FromBody] object json)
        {
            if (json != null)
            {
                JObject jObject = JObject.Parse(json.ToString());
                string id = (string)jObject["id"];
                string orderId = (string)jObject["orderId"];
                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(orderId))
                    throw new Exception("Id and orderid cannot be null/empty.");

                if (jObject["requestTime"] == null)
                    throw new Exception("Request Time cannot be null/empty.");

                string requestTime = Convert.ToDateTime(jObject["requestTime"]).ToString("yyyy-MM-dd HH:mm:ss");

                return new PersistentHelper().DeliveredOrder(id, orderId, requestTime);
                //else
                //    return "Cannot make the order delivered.";
            }
            else
                throw new Exception("Input is null.");
        }
        [HttpPost]
        public string OrderUndelivered([FromBody] object json)
        {
            if (json != null)
            {
                JObject jObject = JObject.Parse(json.ToString());
                string id = (string)jObject["id"];
                string orderId = (string)jObject["orderId"];
                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(orderId))
                    throw new Exception("Id and orderid cannot be null/empty.");

                if (jObject["requestTime"] == null)
                    throw new Exception("Request Time cannot be null/empty.");

                string requestTime = Convert.ToDateTime(jObject["requestTime"]).ToString("yyyy-MM-dd HH:mm:ss");

                return new PersistentHelper().UndeliveredOrder(id, orderId, requestTime);
            }
            else
                throw new Exception("Input is null.");
        }


        [HttpPost]
        public string RevertState([FromBody] object json)
        {
            if (json != null)
            {
                JObject jObject = JObject.Parse(json.ToString());
                string id = (string)jObject["id"];
                string orderId = (string)jObject["orderId"];
                //string requestTime = (string)jObject["requestTime"];
                string revertFromState = (string)jObject["revertFrom"];
                string revertToState = (string)jObject["revertTo"];

                if (string.IsNullOrEmpty(revertFromState) || string.IsNullOrEmpty(revertToState))
                    throw new Exception("revertFrom/revertTo cannot be null/empty.");

                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(orderId))
                    throw new Exception("Id and orderid cannot be null/empty.");

                if (jObject["requestTime"] == null)
                    throw new Exception("Request Time cannot be null/empty.");

                string requestTime = Convert.ToDateTime(jObject["requestTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                return new PersistentHelper().RevertState(id, orderId, requestTime, revertFromState, revertToState);
            }
            else
                throw new Exception("Input is null.");
        }

        [HttpPost]
        public string CancelOrder([FromBody] object json)
        {
            if (json != null)
            {
                JObject jObject = JObject.Parse(json.ToString());
                // JToken jUser = jObject["id"];
                string id = (string)jObject["id"];
                string orderId = (string)jObject["orderId"];
               
                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(orderId))
                    throw new Exception("Id and orderid cannot be null/empty.");

                if (jObject["requestTime"] == null)
                    throw new Exception("Request Time cannot be null/empty.");

                string requestTime = Convert.ToDateTime(jObject["requestTime"]).ToString("yyyy-MM-dd HH:mm:ss");

                return new PersistentHelper().CancelUserOrder(id, orderId, requestTime);
            }
            else
                throw new Exception("Input is null.");
        }

        [HttpPost]
        public string DropAllOrders([FromBody] object json)
        {
            if (json != null)
            {
                JObject jObject = JObject.Parse(json.ToString());
                // JToken jUser = jObject["id"];
                string id = (string)jObject["id"];
                List<string> orderIds = jObject["orderIds"].ToObject<List<string>>();

                if (string.IsNullOrEmpty(id) || orderIds == null)
                    throw new Exception("Id and orderid cannot be null/empty.");

                if (jObject["requestTime"] == null)
                    throw new Exception("Request Time cannot be null/empty.");

                string requestTime = Convert.ToDateTime(jObject["requestTime"]).ToString("yyyy-MM-dd HH:mm:ss");

                return new PersistentHelper().DropAllOrders(id, orderIds, requestTime);
            }
            else
                throw new Exception("Input is null.");
        }


        // Handles delivered and undelivered jobs.
        [HttpPost]
        public string BatchDelivery([FromBody] object json)
        {
            if (json != null)
            {
                JObject jObject = JObject.Parse(json.ToString());
                string id = (string)jObject["id"];

                object undelIdsStr = jObject["undelIds"];
                List<string> undelIds = undelIdsStr == null ? new List<string>() : jObject["undelIds"].ToObject<List<string>>();               

                object delIdsStr =  jObject["delIds"];
                List<string> delIds = delIdsStr == null ? new List<string>() : jObject["delIds"].ToObject<List<string>>();

                string undelivered = String.Join(",", undelIds);
                string delivered = String.Join(",", delIds);


                string requestTime = (string)jObject["requestTime"];

                if (string.IsNullOrEmpty(id))
                    throw new Exception("Id cannot be null/empty.");

                if (string.IsNullOrEmpty(requestTime))
                    throw new Exception("Request Time cannot be null/empty.");

                return new PersistentHelper().BatchUnDelivered(id, undelivered, delivered, requestTime);
            }
            else
                throw new Exception("Input is null.");
        }



        [HttpPost]
        public string PickedupOrder([FromBody] object json)
        {
            if (json != null)
            {
                JObject jObject = JObject.Parse(json.ToString());
                // JToken jUser = jObject["id"];
                string id = (string)jObject["id"];
                string orderId = (string)jObject["orderId"];

                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(orderId))
                    throw new Exception("Id and orderid cannot be null/empty.");

                if (jObject["requestTime"] == null)
                    throw new Exception("Request Time cannot be null/empty.");

                string requestTime = Convert.ToDateTime(jObject["requestTime"]).ToString("yyyy-MM-dd HH:mm:ss");

                return new PersistentHelper().PickedupOrder(id, orderId, requestTime);
            }
            else
                throw new Exception("Input is null.");
        }

        [HttpPost]
        public string HaltOrder([FromBody] object json)
        {
            if (json != null)
            {
                JObject jObject = JObject.Parse(json.ToString());
                // JToken jUser = jObject["id"];
                string id = (string)jObject["id"];
                string orderId = (string)jObject["orderId"];

                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(orderId))
                    throw new Exception("Id and orderid cannot be null/empty.");

                if (jObject["requestTime"] == null)
                    throw new Exception("Request Time cannot be null/empty.");

                string requestTime = Convert.ToDateTime(jObject["requestTime"]).ToString("yyyy-MM-dd HH:mm:ss");

                return new PersistentHelper().HaltOrder(id, orderId, requestTime);
            }
            else
                throw new Exception("Input is null.");
        }

    }
}
