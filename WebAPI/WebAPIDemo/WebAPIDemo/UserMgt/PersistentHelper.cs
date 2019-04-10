using DTO;
using Newtonsoft.Json;
using PersistenceManager;
using Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MySql.Data.MySqlClient;

namespace WebAPIDemo.UserMgt

{
    static class Helper
    {
        public static string GetUntilOrEmpty(this string text, string stopAt = "_")
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return text.Substring(0, charLocation);
                }
            }
            return String.Empty;
        }
    }

    public class PersistentHelper
    {

        internal string UpdateDriverStatus(string id, string state)
        {
            string query = "Update mlo.driver set state = '" + state + "' " + " WHERE id = " + id + " ;";

            List<OrderDetails> orders = new List<OrderDetails>();
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

        internal List<LoginInfo> GetUser(string attributeName, string attributeValue)
        {
            List<List<Dictionary<String, string>>> persistentCarrier = new List<List<Dictionary<String, string>>>();
            string query = "Select * from mlo.users where " + attributeName + " = '" + attributeValue + "'";
            LoginInfo user = null;
            List<LoginInfo> users = new List<LoginInfo>();
            try
            {
                persistentCarrier = DBUtility.ExecuteQuery(query);

                SBMapper map = new SBMapper();
                List<PropertyMap> listPropertyMap = PropertyMapper.MapSignedInUser();
                map.MapperCollection = listPropertyMap;
                foreach (Dictionary<string, string> eachCarrier in persistentCarrier[0])
                {
                    user = new LoginInfo();
                    string json = new DTOMapper().Mapper(eachCarrier, user, map);
                    //Console.WriteLine("Destination = " + (json));
                    user = JsonConvert.DeserializeObject<LoginInfo>(json);
                    user.Name = user.fName + " " + user.last_name;
                    users.Add(user);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return users;
        }

        internal string ChangePassword(string id, string loginType, string currentPwd, string newPwd)
        {
            string query = "";
            switch (loginType.ToLower())
            {
                case "wholesaler":
                case "retailer":
                case "distributor":
                    query = "Update mlo.users set password = '" + newPwd + "'  where id = " + id + " and password = '" + currentPwd + "' ";
                    break;
                case "collector":
                    query = "Update mlo.collector set password = '" + newPwd + "'  where id = " + id + " and password = '" + currentPwd + "' ";
                    break;

                case "loader":
                    query = "Update mlo.loader set password = '" + newPwd + "'  where id = " + id + " and password = '" + currentPwd + "' ";
                    break;
                case "driver":
                    query = "Update mlo.driver set password = '" + newPwd + "'  where id = " + id + " and password = '" + currentPwd + "' ";
                    break;

                default:
                    break;
            }
            try
            {
                int rowsUpdated = DBUtility.ExecuteNonQuery(query);
                if (rowsUpdated >= 1) return "Password changed successfully.";
                else return "Cannot change password";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal object GetLoginDetails(string userId, string loginType)
        {
            LoginInfo loginInfo = null;
            switch (loginType.ToLower())
            {
                case "wholesaler":
                case "retailer":
                case "distributor":
                    List<LoginInfo> users = GetUser("id", userId);
                    loginInfo = users.Count != 0 ? users[0] : null;
                    break;
                case "collector":
                    List<LoginInfo> collectors = GetCollector("id", userId);
                    loginInfo = collectors.Count != 0 ? collectors[0] : null;
                    break;

                case "loader":
                    List<LoginInfo> loaders = GetLoader("id", userId);
                    loginInfo = loaders.Count != 0 ? loaders[0] : null;
                    break;
                case "driver":
                    List<DriverDetails> drivers = GetDriver("id", userId);
                    DriverDetails dd = drivers.Count != 0 ? drivers[0] : null;
                    return dd;
                default:
                    break;
            }
            return loginInfo;
        }
        internal List<LoginInfo> GetAllUsersUsingType(string loginType)
        {
            List<List<Dictionary<String, string>>> persistentCarrier = new List<List<Dictionary<String, string>>>();
            string query = string.Empty;
            if (!string.IsNullOrEmpty(loginType))
                query = "Select * from mlo.users where login_type = '" + loginType + "'";
            else
                query = "Select * from mlo.users";

            LoginInfo user = null;
            List<LoginInfo> users = new List<LoginInfo>();
            try
            {
                persistentCarrier = DBUtility.ExecuteQuery(query);

                SBMapper map = new SBMapper();
                List<PropertyMap> listPropertyMap = PropertyMapper.MapSignedInUser();
                map.MapperCollection = listPropertyMap;
                foreach (Dictionary<string, string> eachCarrier in persistentCarrier[0])
                {
                    user = new LoginInfo();
                    string json = new DTOMapper().Mapper(eachCarrier, user, map);
                    //Console.WriteLine("Destination = " + (json));
                    user = JsonConvert.DeserializeObject<LoginInfo>(json);
                    users.Add(user);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return users;
        }
        internal List<DriverDetails> GetDriver(string state)
        {
            List<List<Dictionary<String, string>>> persistentCarrier = new List<List<Dictionary<String, string>>>();
            string query = "";
            if (string.IsNullOrEmpty(state))
                query = "Select * from mlo.driver;";
            else
            {
                string[] states = state.Split(',');
                string str = "'" + String.Join("','", states) + "'";
                query = "Select * from mlo.driver where state in (" + str + ");";
            }
            DriverDetails driver = null;
            List<DriverDetails> drivers = new List<DriverDetails>();
            try
            {
                persistentCarrier = DBUtility.ExecuteQuery(query);

                SBMapper map = new SBMapper();
                List<PropertyMap> listPropertyMap = PropertyMapper.MapDriverDetails();
                map.MapperCollection = listPropertyMap;
                foreach (Dictionary<string, string> eachCarrier in persistentCarrier[0])
                {
                    driver = new DriverDetails();
                    string json = new DTOMapper().Mapper(eachCarrier, driver, map);
                    //Console.WriteLine("Destination = " + (json));
                    driver = JsonConvert.DeserializeObject<DriverDetails>(json);
                    drivers.Add(driver);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return drivers;
        }
        internal List<DriverDetails> GetDriver(string attributeName, string attributeValue)
        {
            List<List<Dictionary<String, string>>> persistentCarrier = new List<List<Dictionary<String, string>>>();
            string query = "Select * from mlo.driver where " + attributeName + " = '" + attributeValue + "';";
            DriverDetails driver = null;
            List<DriverDetails> drivers = new List<DriverDetails>();
            try
            {
                persistentCarrier = DBUtility.ExecuteQuery(query);

                SBMapper map = new SBMapper();
                List<PropertyMap> listPropertyMap = PropertyMapper.MapDriverDetails();
                map.MapperCollection = listPropertyMap;
                foreach (Dictionary<string, string> eachCarrier in persistentCarrier[0])
                {
                    driver = new DriverDetails();
                    string json = new DTOMapper().Mapper(eachCarrier, driver, map);
                    //Console.WriteLine("Destination = " + (json));
                    driver = JsonConvert.DeserializeObject<DriverDetails>(json);
                    drivers.Add(driver);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return drivers;
        }
        internal List<LoginInfo> GetCollector(string attributeName, string attributeValue)
        {
            List<List<Dictionary<String, string>>> persistentCarrier = new List<List<Dictionary<String, string>>>();
            string query = "Select * from mlo.collector where " + attributeName + " = '" + attributeValue + "';";
            LoginInfo coll = null;
            List<LoginInfo> listColl = new List<LoginInfo>();
            try
            {
                persistentCarrier = DBUtility.ExecuteQuery(query);

                SBMapper map = new SBMapper();
                List<PropertyMap> listPropertyMap = PropertyMapper.MapSignedInCollector();
                map.MapperCollection = listPropertyMap;
                foreach (Dictionary<string, string> eachCarrier in persistentCarrier[0])
                {
                    coll = new LoginInfo();
                    string json = new DTOMapper().Mapper(eachCarrier, coll, map);
                    //Console.WriteLine("Destination = " + (json));
                    coll = JsonConvert.DeserializeObject<LoginInfo>(json);
                    listColl.Add(coll);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listColl;
        }
        internal List<LoginInfo> GetLoader(string attributeName, string attributeValue)
        {
            List<List<Dictionary<String, string>>> persistentCarrier = new List<List<Dictionary<String, string>>>();
            string query = "Select * from mlo.loader where " + attributeName + " = '" + attributeValue + "';";
            LoginInfo loader = null;
            List<LoginInfo> loaders = new List<LoginInfo>();
            try
            {
                persistentCarrier = DBUtility.ExecuteQuery(query);

                SBMapper map = new SBMapper();
                List<PropertyMap> listPropertyMap = PropertyMapper.MapSignedInLoader();
                map.MapperCollection = listPropertyMap;
                foreach (Dictionary<string, string> eachCarrier in persistentCarrier[0])
                {
                    loader = new LoginInfo();
                    string json = new DTOMapper().Mapper(eachCarrier, loader, map);
                    loader = JsonConvert.DeserializeObject<LoginInfo>(json);
                    loaders.Add(loader);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return loaders;
        }


        internal List<LoginInfo> GetEmployee()
        {
            List<List<Dictionary<String, string>>> persistentCarrier = new List<List<Dictionary<String, string>>>();
            string query = "select * from mlo.collector union all select * from mlo.loader union all select * from mlo.driver";
            //string query1 = "select * from mlo.loader";
            //string query2 = "select * from mlo.driver";

            //List<List<Dictionary<String, string>>> persistentCarrier = new List<List<Dictionary<String, string>>>();
            //string query = "Select * from mlo.users where " + attributeName + " = '" + attributeValue + "'";
            LoginInfo employee = null;
            List<LoginInfo> employees = new List<LoginInfo>();
            try
            {
                persistentCarrier = DBUtility.ExecuteQuery(query);
                SBMapper map = new SBMapper();
                List<PropertyMap> listPropertyMap = PropertyMapper.MapSignedInDriver();
                map.MapperCollection = listPropertyMap;
                foreach (Dictionary<string, string> eachCarrier in persistentCarrier[0])
                {
                    employee = new LoginInfo();
                    string json = new DTOMapper().Mapper(eachCarrier, employee, map);
                    //Console.WriteLine("Destination = " + (json));
                    employee = JsonConvert.DeserializeObject<LoginInfo>(json);
                    employees.Add(employee);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return employees;
        }

        internal LoginInfo SignIn(string userId, String password)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(password))
                throw new ArgumentException("User Id or password is empty!");

            LoginInfo user = null;
            try
            {
                String query = string.Empty;
                String login_type = string.Empty;
                String pageToLoad = string.Empty;
                List<PropertyMap> listPropertyMap = null;
                switch (userId.GetUntilOrEmpty().ToLower())
                {
                    case "di":
                        login_type = "distributor";
                        pageToLoad = "request_delivery.aspx";
                        query = "Select * from mlo.users where login_id='" + userId + "' and password='" + password + "' and active=1";
                        listPropertyMap = PropertyMapper.MapSignedInUser();
                        break;
                    case "wh":
                        login_type = "wholesaler";
                        pageToLoad = "request_delivery.aspx";
                        query = "Select * from mlo.users where login_id='" + userId + "' and password='" + password + "' and active=1";
                        listPropertyMap = PropertyMapper.MapSignedInUser();
                        break;
                    case "re":
                        login_type = "retailer";
                        pageToLoad = "request_delivery.aspx";
                        query = "Select * from mlo.users where login_id='" + userId + "' and password='" + password + "' and active=1";
                        listPropertyMap = PropertyMapper.MapSignedInUser();
                        break;
                    case "co":
                        login_type = "collector";
                        pageToLoad = "pickup_list.aspx";
                        query = "Select * from mlo.collector where login_id='" + userId + "' and password='" + password + "' and active=1";
                        listPropertyMap = PropertyMapper.MapSignedInCollector();
                        break;
                    case "lo":
                        login_type = "loader";
                        pageToLoad = "warehouse_status.aspx";
                        query = "Select * from mlo.loader where login_id='" + userId + "' and password='" + password + "' and active=1";
                        listPropertyMap = PropertyMapper.MapSignedInLoader();
                        break;
                    case "dr":
                        login_type = "driver";
                        pageToLoad = "driver_route.aspx";
                        query = "Select * from mlo.driver where login_id='" + userId + "' and password='" + password + "' and active=1";
                        listPropertyMap = PropertyMapper.MapSignedInDriver();
                        break;
                    default:
                        throw new Exception("Login Id not in proper format.");
                }
                List<List<Dictionary<String, string>>> persistentCarrier = new List<List<Dictionary<String, string>>>();
                persistentCarrier = DBUtility.ExecuteQuery(query);

                if (persistentCarrier[0].Count == 0)
                    throw new Exception("Invalid credentials.");

                else if (persistentCarrier[0].Count == 1)
                {
                    SBMapper map = new SBMapper();
                    map.MapperCollection = listPropertyMap;
                    Dictionary<string, string> eachCarrier = persistentCarrier[0][0];
                    user = new LoginInfo();
                    string json = new DTOMapper().Mapper(eachCarrier, user, map);
                    user = JsonConvert.DeserializeObject<LoginInfo>(json);
                    user.LoginType = login_type;
                    user.PageToLoad = pageToLoad;
                }
                else
                {
                    throw new Exception("There are multiple users with specified login id");
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //throw new WebFaultException<CustomFault>(new CustomFault("Error in SignIn ", skyboInternalSvrErr, ex.Message), internalSvrErr);
            }
            return user;
        }

        internal string AvailSubscription(string userId, string subsId, string date)
        {
            string query = "Insert into mlo.user_subscription('user_id', 'subs_id', 'deliveries', 'start_date') " +
                            " VALUES (?, ?, ?, ?)";
            query = query + " Select LAST_INSERT_ID() as id;";
            List<MySqlParameter> prm = new List<MySqlParameter>()
            {
            new MySqlParameter("@user_id", MySqlDbType.VarChar) {Value = userId},
            new MySqlParameter("@subs_id", MySqlDbType.VarChar) {Value = subsId},
            new MySqlParameter("@deliveries", MySqlDbType.VarChar) {Value = "0"},
            new MySqlParameter("@start_date", MySqlDbType.VarChar) {Value = date}
            };

            List<List<Dictionary<String, string>>> persistentCarrier = new List<List<Dictionary<String, string>>>();
            try
            {
                persistentCarrier = DBUtility.ExecuteQuery(query);

                if (persistentCarrier[0][0].TryGetValue("id", out userId))
                {
                    if (string.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unable to Sign up. UserId did not get generated.");
                    }
                } 

            }
            catch (Exception ex)
            {
                //Logger.Log(Level.Error, "Error in Sign Up :: " + ex.Message + "\n Caused By :- " + ex.StackTrace);
                //throw new WebFaultException<CustomFault>(new CustomFault("Error in Sign Up ", skyboInternalSvrErr, ex.Message), internalSvrErr);
                throw ex;
            }
            throw new NotImplementedException();
        }

        internal string SignUp(LoginInfo userInfo, String query, List<MySqlParameter> values)
        {
            //Logger.Log(Level.Debug, "Sign Up..!");

            string userId = "0";
            List<List<Dictionary<String, string>>> persistentCarrier = new List<List<Dictionary<String, string>>>();
            try
            {
                persistentCarrier = DBUtility.ExecuteQuery(query, values);

                if (persistentCarrier[0][0].TryGetValue("id", out userId))
                {
                    if (string.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unable to Sign up. UserId did not get generated.");
                    }
                }

            }
            catch (Exception ex)
            {
                //Logger.Log(Level.Error, "Error in Sign Up :: " + ex.Message + "\n Caused By :- " + ex.StackTrace);
                //throw new WebFaultException<CustomFault>(new CustomFault("Error in Sign Up ", skyboInternalSvrErr, ex.Message), internalSvrErr);
                throw ex;
            }
            return userId;
        }
    }
}