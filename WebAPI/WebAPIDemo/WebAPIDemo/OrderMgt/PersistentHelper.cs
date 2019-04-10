using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO;
using PersistenceManager;
using Utility;
using Newtonsoft.Json;
using System.Globalization;
using Newtonsoft.Json.Converters;
using System.Text;
using System.Net;
using MySql.Data.MySqlClient;
using System.Collections;
using log4net.Core;

namespace WebAPIDemo.OrderMgt
{
    public class PersistentHelper
    {
        internal string CreateOrder(OrderDetails order)
        {

            DateTime dt = (DateTime)order.requestTime;

            string query = "INSERT INTO `mlo`.`orders`(user_id,destination,latitude,longitude, loc_coordinates, loc_id, challan_no,no_of_Items,request_time,state,active) VALUES (?,?,?,?,?,?,?,?,?,?,?)";

            List<MySqlParameter> prm = new List<MySqlParameter>()
                {
                    new MySqlParameter("@user_id", MySqlDbType.Int32) {Value = order.userId},
                    new MySqlParameter("@destination", MySqlDbType.Text) {Value = order.destination},
                    new MySqlParameter("@latitude", MySqlDbType.Decimal) {Value = order.latitude},
                    new MySqlParameter("@longitude", MySqlDbType.Decimal) {Value = order.longitude},
                    new MySqlParameter("@loc_coordinates", MySqlDbType.Text) {Value = order.locationCoordinates},
                    new MySqlParameter("@loc_id", MySqlDbType.Text) {Value = order.locId},
                    new MySqlParameter("@challan_no", MySqlDbType.VarChar) {Value = order.challanNumber},
                    new MySqlParameter("@no_of_Items", MySqlDbType.Int16) {Value = order.noOfItems},
                    new MySqlParameter("@request_time", MySqlDbType.DateTime) {Value = dt.ToString("yyyy-MM-dd HH:mm:ss")},
                    new MySqlParameter("@state", MySqlDbType.VarChar) {Value = Constants.Initiated},
                    new MySqlParameter("@active", MySqlDbType.VarChar) {Value = "YES"}
                };

            try
            {
                DBUtility.ExecuteQuery(query, prm);
            }
            catch (Exception ex)
            {
                //Logger.Log(Level.Error, "Error in CreateOrder :: " + ex.Message + "\n Caused By :- " + ex.StackTrace);
                //throw new WebFaultException<CustomFault>(new CustomFault("Error in Sign Up ", skyboInternalSvrErr, ex.Message), internalSvrErr);
                throw ex;
            }
            return "Order placed successfully";
        }

        internal bool CheckStatus(string orderId, string status)
        {
            bool exist = true;
            List<List<Dictionary<String, string>>> persistentCarrier = new List<List<Dictionary<String, string>>>();
            string query = "SELECT state FROM mlo.orders where id = '" + orderId + "' and state='" + status + "'";
            try
            {
                persistentCarrier = DBUtility.ExecuteQuery(query);
                if (persistentCarrier[0].Count == 0)
                    exist = false;
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot check status");
            }
            return exist;
        }



        internal OrderDetails GetSpecificOrder(string destination, string challanNumber)
        {

            List<List<Dictionary<String, string>>> persistentCarrier = new List<List<Dictionary<String, string>>>();
            string query = "SELECT id FROM mlo.orders where destination = '" + destination + "' and challan_no='" + challanNumber + "'";
            OrderDetails order = null;

            try
            {
                persistentCarrier = DBUtility.ExecuteQuery(query);
                SBMapper map = new SBMapper();
                List<PropertyMap> listPropertyMap = PropertyMapper.MapOrderId();
                map.MapperCollection = listPropertyMap;
                foreach (Dictionary<string, string> eachCarrier in persistentCarrier[0])
                {
                    order = new OrderDetails();
                    string json = new DTOMapper().Mapper(eachCarrier, order, map);
                    //console.WriteLine("Destination = " + (json));
                    order = JsonConvert.DeserializeObject<OrderDetails>(json);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return order;
        }

        internal string DropAllOrders(string id, List<string> orderIds, string date)
        {
            string orders = String.Join(",", orderIds);
            //DateTime timeStamp = new DateTime();
            //timeStamp = Convert.ToDateTime(Util.getHeaderValue("Date"));

            date = Convert.ToDateTime(date).ToString("yyyy-MM-dd HH:mm:ss");
            string query = "Update mlo.orders set  state = '" + Constants.Dropped + "' , drop_time='" + date
                            + "' WHERE id in (" + orders + ") and active = 'YES' and state = '" + Constants.Pickedup + "' and pickup_by = '" + id + "' ; ";

            try
            {
                int rowsUpdated = DBUtility.ExecuteNonQuery(query);
                if (rowsUpdated >= 1) return "Success";
                else return "No row updated.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal string BatchUnDelivered(string id, string undelivered, string delivered, string date)
        {

            date = Convert.ToDateTime(date).ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                if (!string.IsNullOrEmpty(undelivered))
                {
                    string query = "Update mlo.orders set state = '" + Constants.Undelivered + "' WHERE id in (" + undelivered + ") and active = 'YES';";
                    DBUtility.ExecuteQuery(query);
                }
                if (!string.IsNullOrEmpty(delivered))
                {
                    string query1 = "Update mlo.orders set active = 'NO' , state = '" + Constants.Delivered + "', delivered_time = '" + date + "' WHERE id in (" + delivered + ") and active = 'YES';";
                    DBUtility.ExecuteQuery(query1);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "Success";
        }

        internal string PickedupOrder(string id, string orderId, string date)
        {
            //DateTime timeStamp = new DateTime();
            //timeStamp = Convert.ToDateTime(Util.getHeaderValue("Date"));
            // ,
            date = Convert.ToDateTime(date).ToString("yyyy-MM-dd HH:mm:ss");
            string query = "Update mlo.orders set  state = '" + Constants.Pickedup + "' , pickup_time='" + date
                + "' WHERE id = '" + orderId + "'  and state = '" + Constants.Accepted + "' and active = 'YES' and pickup_by = '" + id + "';";

            try
            {
                int rowsUpdated = DBUtility.ExecuteNonQuery(query);
                if (rowsUpdated >= 1) return "Success";
                else return "No row updated.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal string HaltOrder(string id, string orderId, string date)
        {
            //DateTime timeStamp = new DateTime();
            //timeStamp = Convert.ToDateTime(Util.getHeaderValue("Date"));
            // ,
            date = Convert.ToDateTime(date).ToString("yyyy-MM-dd HH:mm:ss");
            string query = "Update mlo.orders set  state = '" + Constants.Halted + "' WHERE id = " + orderId + "  and state = '" + Constants.Dropped + "' and active = 'YES' ;";
            try
            {
                int rowsUpdated = DBUtility.ExecuteNonQuery(query);
                if (rowsUpdated >= 1) return "Success";
                else return "No row updated.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal string RevertState(string id, string orderId, string requestTime, string revertFrom, string revertTo)
        {
            return UpdateOrder1(id, orderId, requestTime, revertFrom, revertTo);
        }
        internal string CancelUserOrder(string id, string orderId, string requestTime)
        {
            //DateTime timeStamp = new DateTime();
            //timeStamp = Convert.ToDateTime(Util.getHeaderValue("Date"));
            //Logger.Log(Level.Debug, "query11" + status);
            requestTime = Convert.ToDateTime(requestTime).ToString("yyyy-MM-dd HH:mm:ss");
            string query = "Update mlo.orders set active = '" + "NO" + "' , state = '" + Constants.Cancelled + "' WHERE id = '" + orderId
                            + "' and active = 'YES' and state = '" + Constants.Initiated + "'; ";
            //Logger.Log(Level.Debug, "query" + query
            try
            {
                int rowsUpdated = DBUtility.ExecuteNonQuery(query);
                if (rowsUpdated >= 1) return "Order Cancelled";
                else return "Cannot cancel the order.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal string AcceptOrder(string id, string orderId, string requestTime)
        {
            requestTime = Convert.ToDateTime(requestTime).ToString("yyyy-MM-dd HH:mm:ss");
            string query = "Update mlo.orders set pickup_by = '" + id + "' , state = '" + Constants.Accepted + "' , pickup_time = '" + requestTime
                                    + "' WHERE id = '" + orderId + "' and active = 'YES' and state = '" + Constants.Initiated + "' ;";
            try
            {
                int rowsUpdated = DBUtility.ExecuteNonQuery(query);
                if (rowsUpdated >= 1) return "Success";
                else return "Cannot accept the order.";
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        internal string DeliveredOrder(string id, string orderId, string requestTime)
        {
            requestTime = Convert.ToDateTime(requestTime).ToString("yyyy-MM-dd HH:mm:ss");
            string query = "Update mlo.orders set state = '" + Constants.Delivered + "', delivered_time = '" + requestTime + "' , active='NO' " +
                                    "WHERE id = '" + orderId + "' and active = 'YES' and state in ('" + Constants.OutForDelivery + "', '" + Constants.Undelivered + "') ;";

            try
            {
                int rowsUpdated = DBUtility.ExecuteNonQuery(query);
                if (rowsUpdated >= 1) return "Success";
                else return "Cannot make the order delivered.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal string UndeliveredOrder(string id, string orderId, string requestTime)
        {
            requestTime = Convert.ToDateTime(requestTime).ToString("yyyy-MM-dd HH:mm:ss");
            string query = "Update mlo.orders set state = '" + Constants.Undelivered + "' ,active = 'YES'" +
                                    "WHERE id = '" + orderId + "'  and state in ('" + Constants.OutForDelivery + "', '"+Constants.Delivered+"') ;";
            try
            {
                int rowsUpdated = DBUtility.ExecuteNonQuery(query);
                if (rowsUpdated >= 1) return "Success";
                else return "Cannot make the order undelivered.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal string UpdateUserOrder(string id, string orderId, string requestTime, string status)
        {
            //DateTime timeStamp = new DateTime();
            //timeStamp = Convert.ToDateTime(Util.getHeaderValue("Date"));
            //Logger.Log(Level.Debug, "query11" + status);
            requestTime = Convert.ToDateTime(requestTime).ToString("yyyy-MM-dd HH:mm:ss");
            string query = "Update mlo.orders set active = '" + "NO" + "' , state = '" + status + "' WHERE id = '" + orderId + "';";
            //Logger.Log(Level.Debug, "query" + query);
            try
            {
                DBUtility.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "Success";

        }
        internal string UpdateOrder1(string id, string orderId, string requestTime, string revertFrom, string revertTo)
        {
            //Logger.Log(Level.Debug, "query11" + status);
            string query = string.Empty;
            requestTime = Convert.ToDateTime(requestTime).ToString("yyyy-MM-dd HH:mm:ss");
            query = "Update mlo.orders set state = '" + revertTo + "', active='YES' WHERE id = '" + orderId + "' and state = '" + revertFrom + "'  ;";

            //if (revertTo.Equals(Constants.OutForDelivery))
            //{
            //    query = "Update mlo.orders set state = '" + revertTo + "', active='YES' WHERE id = '" + orderId + "'  ;";
            //}
            //else if (revertTo.Equals(Constants.Delivered))
            //{
            //    query = "Update mlo.orders set state = '" + revertTo + "'WHERE id = '" + orderId + "';";
            //}
            //else if (revertTo.Equals(Constants.Initiated))
            //{
            //    query = "Update mlo.orders set state = '" + revertTo + "'WHERE id = '" + orderId + "';";
            //}

            //Logger.Log(Level.Debug, "query" + query);
            try
            {
                int rowsUpdated = DBUtility.ExecuteNonQuery(query);
                if (rowsUpdated >= 1) return "Success";
                else return "Cannot update the order.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // userid is not mandatory
        internal List<OrderDetails> GetOrderForWarehouse(string userId, string state)
        {
            string query = string.Empty;


            if (string.IsNullOrEmpty(state))
            {
                // query = "SELECT * FROM mlo.orders order by id desc";
                //query = "SELECT CONCAT(u.first_name, ' ', u.surname) wholesaler_name, o.* FROM mlo.orders o join mlo.users u on o.user_id = u.id ";
                query = "SELECT CONCAT(u.first_name, ' ', u.surname) wholesaler_name, u.firm_name , CONCAT(c.first_name, ' ', c.surname) as pickup_by_name, " +
                        "o.* FROM mlo.orders o left join mlo.users u on o.user_id = u.id "
                        + " left join mlo.collector c on o.pickup_by = c.id  where o.active = 'YES' ";
            }
            else
            {
                string[] states = state.Split(',');
                string str = "'" + String.Join("','", states) + "'";
                // query = "SELECT * FROM mlo.orders where state in (" + str + ") order by id desc";
                // query = "SELECT CONCAT(u.first_name, ' ', u.surname) wholesaler_name, o.* FROM mlo.orders o join mlo.users u on o.user_id = u.id where state in (" + str + ")";
                query = "SELECT CONCAT(u.first_name, ' ', u.surname) wholesaler_name, u.firm_name ,  CONCAT(c.first_name, ' ', c.surname) as pickup_by_name, " +
                       "o.* FROM mlo.orders o left join mlo.users u on o.user_id = u.id "
                       + " left join mlo.collector c on o.pickup_by = c.id  where state in (" + str + ")  and o.active = 'YES'";
            }
            List<List<Dictionary<String, string>>> persistentCarrier = new List<List<Dictionary<String, string>>>();
            List<OrderDetails> orders = new List<OrderDetails>();
            try
            {
                persistentCarrier = DBUtility.ExecuteQuery(query);

                SBMapper map = new SBMapper();
                List<PropertyMap> listPropertyMap = PropertyMapper.MapOrderForWholesaler();
                map.MapperCollection = listPropertyMap;
                OrderDetails order = null;
                foreach (Dictionary<string, string> eachCarrier in persistentCarrier[0])
                {
                    order = new OrderDetails();
                    string json = new DTOMapper().Mapper(eachCarrier, order, map);

                    var format = "dd-MM-yyyy HH:mm:ss"; // your datetime format
                    var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
                    order = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderDetails>(json, dateTimeConverter);
                    //order = JsonConvert.DeserializeObject<OrderDetails>(json);
                    orders.Add(order);
                }

            }
            catch (Exception ex)
            {
                var lineNumber = 0;
                const string lineSearch = ":line ";
                var index = ex.StackTrace.LastIndexOf(lineSearch);
                var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                if (int.TryParse(lineNumberText, out lineNumber))
                {
                }
                throw new Exception(ex.ToString() + " \n line number " + lineNumber + " \n line number text " + lineNumberText);
            }
            return orders;
        }

        internal List<OrderDetails> GetAllOrders(string userId, string state)
        {
            string query = string.Empty;


            if (string.IsNullOrEmpty(state))
            {
                // query = "SELECT * FROM mlo.orders order by id desc";
                //query = "SELECT CONCAT(u.first_name, ' ', u.surname) wholesaler_name, o.* FROM mlo.orders o join mlo.users u on o.user_id = u.id ";
                query = "SELECT CONCAT(u.first_name, ' ', u.surname) wholesaler_name,u.firm_name , CONCAT(c.first_name, ' ', c.surname) as pickup_by_name, " +
                        "o.* FROM mlo.orders o left join mlo.users u on o.user_id = u.id "
                        + " left join mlo.collector c on o.pickup_by = c.id  ";
            }
            else
            {
                string[] states = state.Split(',');
                string str = "'" + String.Join("','", states) + "'";
                // query = "SELECT * FROM mlo.orders where state in (" + str + ") order by id desc";
                // query = "SELECT CONCAT(u.first_name, ' ', u.surname) wholesaler_name, o.* FROM mlo.orders o join mlo.users u on o.user_id = u.id where state in (" + str + ")";
                query = "SELECT CONCAT(u.first_name, ' ', u.surname) wholesaler_name,u.firm_name , CONCAT(c.first_name, ' ', c.surname) as pickup_by_name, " +
                       "o.* FROM mlo.orders o left join mlo.users u on o.user_id = u.id "
                       + " left join mlo.collector c on o.pickup_by = c.id  where state in (" + str + ")";
            }
            List<List<Dictionary<String, string>>> persistentCarrier = new List<List<Dictionary<String, string>>>();
            List<OrderDetails> orders = new List<OrderDetails>();
            try
            {
                persistentCarrier = DBUtility.ExecuteQuery(query);

                SBMapper map = new SBMapper();
                List<PropertyMap> listPropertyMap = PropertyMapper.MapOrderForWholesaler();
                map.MapperCollection = listPropertyMap;
                OrderDetails order = null;
                foreach (Dictionary<string, string> eachCarrier in persistentCarrier[0])
                {
                    order = new OrderDetails();
                    string json = new DTOMapper().Mapper(eachCarrier, order, map);

                    var format = "dd-MM-yyyy HH:mm:ss"; // your datetime format
                    var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
                    order = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderDetails>(json, dateTimeConverter);
                    //order = JsonConvert.DeserializeObject<OrderDetails>(json);
                    orders.Add(order);
                }

            }
            catch (Exception ex)
            {
                var lineNumber = 0;
                const string lineSearch = ":line ";
                var index = ex.StackTrace.LastIndexOf(lineSearch);
                var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                if (int.TryParse(lineNumberText, out lineNumber))
                {
                }
                throw new Exception(ex.ToString() + " \n line number " + lineNumber + " \n line number text " + lineNumberText);
            }
            return orders;
        }



        internal List<OrderDetails> GetOrderForUser(string userId, string state)
        {

            string query = string.Empty;

            if (string.IsNullOrEmpty(userId))
            {
                if (string.IsNullOrEmpty(state))
                   // query = "SELECT * FROM mlo.orders order by id desc";
                query = "SELECT CONCAT(u.first_name, ' ', u.surname) wholesaler_name, u.firm_name, u.shop_no, o.* " +
                         "FROM mlo.orders o " +
                         "left join mlo.users u " +
                         "on o.user_id = u.id " +
                         //"where o.active = 'YES' " +
                         "order by o.id desc";
                else
                {
                    string[] states = state.Split(',');
                    string str = "'" + String.Join("','", states) + "'";
                   // query = "SELECT * FROM mlo.orders where state in (" + str + ") order by id desc";
                    query = "SELECT CONCAT(u.first_name, ' ', u.surname) wholesaler_name, u.firm_name, u.shop_no, o.* " +
                        "FROM mlo.orders o " +
                        "left join mlo.users u " +
                        "on o.user_id = u.id " +
                        "where state in (" + str + ") " +
                        "order by o.id desc";

                }
            }
            else
            {
                if (string.IsNullOrEmpty(state))
                    //query = "SELECT * FROM mlo.orders where user_id = '" + userId + "' order by id desc";
                    query = "SELECT CONCAT(u.first_name, ' ', u.surname) wholesaler_name, u.firm_name, u.shop_no , o.* " +
                       "FROM mlo.orders o " +
                       "left join mlo.users u " +
                       "on o.user_id = u.id " +
                       "where user_id = '" + userId + "' " +
                       "order by o.id desc";
                else
                {
                    string[] states = state.Split(',');
                    string str = "'" + String.Join("','", states) + "'";
                   // query = "SELECT * FROM mlo.orders where user_id = '" + userId + "' and state in (" + str + ") order by id desc";
                    query = "SELECT CONCAT(u.first_name, ' ', u.surname) wholesaler_name, u.firm_name, u.shop_no, o.* " +
                        "FROM mlo.orders o " +
                        "left join mlo.users u " +
                        "on o.user_id = u.id " +
                        "where state in (" + str + ") and user_id = '" + userId + "' " +
                        "order by o.id desc";
                }
            }

            List<List<Dictionary<String, string>>> persistentCarrier = new List<List<Dictionary<String, string>>>();
            List<OrderDetails> orders = new List<OrderDetails>();
            try
            {
                persistentCarrier = DBUtility.ExecuteQuery(query);

                SBMapper map = new SBMapper();
                List<PropertyMap> listPropertyMap = PropertyMapper.MapOrderForUser();
                map.MapperCollection = listPropertyMap;
                OrderDetails order = null;
                foreach (Dictionary<string, string> eachCarrier in persistentCarrier[0])
                {
                    order = new OrderDetails();
                    string json = new DTOMapper().Mapper(eachCarrier, order, map);
                    //var format = "dd-MM-yyyy HH:mm:ss"; // your datetime format
                    //var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
                    var dateTimeConverter = new IsoDateTimeConverter
                    { Culture = CultureInfo.CurrentCulture };

                    order = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderDetails>(json, dateTimeConverter);
                    //order = JsonConvert.DeserializeObject<OrderDetails>(json);
                    orders.Add(order);
                }

            }
            catch (Exception ex)
            {
                //var lineNumber = 0;
                //const string lineSearch = ":line ";
                //var index = ex.StackTrace.LastIndexOf(lineSearch);
                //var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                //if (int.TryParse(lineNumberText, out lineNumber))
                //{
                //}
                //Logger.Log(Level.Error,"Error in get order " + ex.StackTrace);
                //throw new Exception(ex.ToString() + " \n line number " + lineNumber + " \n line number text " + lineNumberText);
                throw ex;
            }
            return orders;
        }

        internal List<OrderDetails> GetRouteOrders(string id, string driverId)
        {
            List<List<Dictionary<String, string>>> persistentCarrier = new List<List<Dictionary<String, string>>>();
            List<OrderDetails> orders = new List<OrderDetails>();
            try
            {
                string query = "SELECT route FROM mlo.driver where id = " + driverId;
                persistentCarrier = DBUtility.ExecuteQuery(query);

                if (persistentCarrier[0].Count > 0)
                {
                    Dictionary<string, string> driverDetails = persistentCarrier[0][0];
                    string route = "";
                   if(driverDetails.TryGetValue("route", out route))
                    {
                        query = "SELECT * FROM orders where id in (" + route + ")  ORDER BY FIELD(id, " + route + ")";
                        try
                        {
                            persistentCarrier = DBUtility.ExecuteQuery(query);

                            SBMapper map = new SBMapper();
                            List<PropertyMap> listPropertyMap = PropertyMapper.MapOrderForEmployee();
                            map.MapperCollection = listPropertyMap;
                            OrderDetails order = null;
                            foreach (Dictionary<string, string> eachCarrier in persistentCarrier[0])
                            {
                                order = new OrderDetails();
                                string json = new DTOMapper().Mapper(eachCarrier, order, map);
                                var format = "dd-MM-yyyy HH:mm:ss"; 
                                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
                                order = JsonConvert.DeserializeObject<OrderDetails>(json, dateTimeConverter);
                                orders.Add(order);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                   else
                    {
                        throw new Exception("Cannot find drivers route details.");
                    }

                }
                else
                {
                    throw new Exception("Incorrect driver id.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return orders;
        }
        internal List<OrderDetails> GetOrderForEmployee(string empType, string id, string state)
        {
            string query = string.Empty;

            string attributeName = string.Empty;
            empType = empType.ToLower();

            switch (empType)
            {
                case "collector":
                    attributeName = "pickup_by";
                    break;

                case "loader":
                    attributeName = "loaded_by";
                    break;

                case "driver":
                    attributeName = "driver";
                    break;
            }

            if (string.IsNullOrEmpty(state))
                query = "SELECT * FROM mlo.orders where " + attributeName + " = '" + id + "' and active='YES' order by id desc";
            else
            {
                string[] states = state.Split(',');
                string str = "'" + String.Join("','", states) + "'";

                query = "SELECT * FROM mlo.orders where " + attributeName + " = '" + id + "' and state in (" + str + ") and active='YES' order by id desc";
                //Logger.Log(Level.Debug, query);
            }

            List<List<Dictionary<String, string>>> persistentCarrier = new List<List<Dictionary<String, string>>>();
            List<OrderDetails> orders = new List<OrderDetails>();
            try
            {
                persistentCarrier = DBUtility.ExecuteQuery(query);

                SBMapper map = new SBMapper();
                List<PropertyMap> listPropertyMap = PropertyMapper.MapOrderForEmployee();
                map.MapperCollection = listPropertyMap;
                OrderDetails order = null;
                foreach (Dictionary<string, string> eachCarrier in persistentCarrier[0])
                {
                    order = new OrderDetails();
                    string json = new DTOMapper().Mapper(eachCarrier, order, map);
                    //console.WriteLine("Destination = " + (json));

                    var format = "dd-MM-yyyy HH:mm:ss"; // your datetime format
                                                        //var format = "yyyy-MM-dd HH:mm:ss"; // your datetime format
                    var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
                    order = JsonConvert.DeserializeObject<OrderDetails>(json, dateTimeConverter);
                    //order = JsonConvert.DeserializeObject<OrderDetails>(json);
                    orders.Add(order);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return orders;
        }



        //  not used
        internal List<OrderDetails> GetOrder(string attributeName, string attributeValue)
        {
            string query = string.Empty;

            query = "SELECT * FROM mlo.orders where " + attributeName + " = '" + attributeValue + "' order by id desc";

            List<List<Dictionary<String, string>>> persistentCarrier = new List<List<Dictionary<String, string>>>();
            List<OrderDetails> orders = new List<OrderDetails>();
            try
            {
                persistentCarrier = DBUtility.ExecuteQuery(query);

                SBMapper map = new SBMapper();
                List<PropertyMap> listPropertyMap = PropertyMapper.MapOrder();
                map.MapperCollection = listPropertyMap;
                OrderDetails order = null;
                foreach (Dictionary<string, string> eachCarrier in persistentCarrier[0])
                {
                    order = new OrderDetails();
                    string json = new DTOMapper().Mapper(eachCarrier, order, map);
                    //console.WriteLine("Destination = " + (json));

                    var format = "dd-MM-yyyy HH:mm:ss"; // your datetime format
                    var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
                    order = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderDetails>(json, dateTimeConverter);



                    //order = JsonConvert.DeserializeObject<OrderDetails>(json);
                    orders.Add(order);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return orders;
        }

        //  not used
        internal List<OrderDetails> GetAcceptedOrder(string id)
        {
            string query = string.Empty;
            query = "SELECT * FROM mlo.orders where pickup_by = '" + id + " and state in order by id desc";


            List<List<Dictionary<String, string>>> persistentCarrier = new List<List<Dictionary<String, string>>>();
            List<OrderDetails> orders = new List<OrderDetails>();
            try
            {
                persistentCarrier = DBUtility.ExecuteQuery(query);

                SBMapper map = new SBMapper();
                List<PropertyMap> listPropertyMap = PropertyMapper.MapOrder();
                map.MapperCollection = listPropertyMap;
                OrderDetails order = null;
                foreach (Dictionary<string, string> eachCarrier in persistentCarrier[0])
                {
                    order = new OrderDetails();
                    string json = new DTOMapper().Mapper(eachCarrier, order, map);
                    //console.WriteLine("Destination = " + (json));

                    var format = "dd-MM-yyyy HH:mm:ss"; // your datetime format
                    var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
                    order = JsonConvert.DeserializeObject<OrderDetails>(json, dateTimeConverter);
                    //order = JsonConvert.DeserializeObject<OrderDetails>(json);
                    orders.Add(order);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return orders;
        }

    }
}
