using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DTO;
using WebAPIDemo.UserMgt;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;
using Utility;
using log4net.Core;
using PersistenceManager;

namespace WebAPIDemo.Controllers
{
    public class UserManagementController : ApiController
    {

        [HttpPost]
        public string UpdateDriverStatus([FromBody] object json)
        {
            JObject jObject = JObject.Parse(json.ToString());
            string id = (string)jObject["id"];
            string state = (string)jObject["state"];

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(state))
            { throw new Exception("Id or state cannot be null/empty"); }

            return new PersistentHelper().UpdateDriverStatus(id, state);
        }

        [HttpPost]
        public string UpdateUserDetails([FromBody] LoginInfo loginInfo)
        {
            string query = "";

            string name = loginInfo.Name;
            loginInfo.fName = name.Split(' ')[0];
            loginInfo.last_name = name.Split(' ')[1];
            switch (loginInfo.LoginType.ToLower())
            {
                case "wholesaler":
                case "retailer":
                case "distributor":
                    query = "Update mlo.users set first_name = '" + loginInfo.fName + "', surname = '" + loginInfo.last_name + "', firm_name = '" + loginInfo.firm_name + "', "
                        + "  address = '" + loginInfo.FirmAddress + "' , gst_no = '" + loginInfo.GSTNo + "' , mobile_no = '" + loginInfo.MobileNumber + "', email_id = '" + loginInfo.EMailId + "' "
                        + "    where id = " + loginInfo.Id + " ";
                    break;
                case "collector":
                    //query = "Update mlo.collector set password = '" + newPwd + "'  where id = " + id + " and password = '" + currentPwd + "' ";
                    break;

                case "loader":
                    // query = "Update mlo.loader set password = '" + newPwd + "'  where id = " + id + " and password = '" + currentPwd + "' ";
                    break;
                case "driver":
                    // query = "Update mlo.driver set password = '" + newPwd + "'  where id = " + id + " and password = '" + currentPwd + "' ";
                    break;

                default:
                    break;
            }


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

        [HttpPost]
        public string ChangePassword([FromBody] object json)
        {
            if (json != null)
            {
                JObject jObject = JObject.Parse(json.ToString());
                string id = (string)jObject["id"];
                string loginType = (string)jObject["loginType"];
                string currentPwd = (string)jObject["currentPwd"];
                string newPwd = (string)jObject["newPwd"];

                if (string.IsNullOrEmpty(id))
                { throw new Exception("Id cannot be null/empty"); }

                if (string.IsNullOrEmpty(loginType))
                { throw new Exception("LoginType cannot be null/empty"); }

                if (string.IsNullOrEmpty(currentPwd))
                { throw new Exception("Current password cannot be null/empty"); }

                if (string.IsNullOrEmpty(newPwd))
                { throw new Exception("New password cannot be null/empty"); }


                return new PersistentHelper().ChangePassword(id, loginType, currentPwd, newPwd);
            }
            else
                throw new ArgumentException("Input is null!");
        }


        public List<DriverDetails> GetDriver(string state = "")
        {
            return new PersistentHelper().GetDriver(state);
        }


        public List<LoginInfo> GetUsers(string loginType = null)
        {
            return new PersistentHelper().GetAllUsersUsingType(loginType);
        }

        //Not Working
        public List<LoginInfo> GetEmployee()
        {
            return new PersistentHelper().GetEmployee();
        }

        public object GetUserDetails(string userId, string loginType)
        {
            //if (json != null)
            //{
            //JObject jObject = JObject.Parse(json.ToString());
            //string userId = (string)jObject["userId"];
            //string loginType = (string)jObject["loginType"];
            return new PersistentHelper().GetLoginDetails(userId, loginType);
            //}
            //else
            //    throw new ArgumentException("Input is null!");
        }



        [HttpPost]
        public LoginInfo SignIn([FromBody] object json)
        {
            if (json != null)
            {
                JObject jObject = JObject.Parse(json.ToString());
                string userId = (string)jObject["userId"];
                string password = (string)jObject["password"];
                return new PersistentHelper().SignIn(userId, password);
            }
            else
            {
                Logger.Log(Level.Error, "Input is null");
                throw new ArgumentException("Input is null!");
            }
        }

        [HttpPost]
        public string SignUp([FromBody]LoginInfo loginInfo)
        {
            if (loginInfo != null)
            {
                if (!string.IsNullOrEmpty(loginInfo.LoginId) && !string.IsNullOrEmpty(loginInfo.LoginType))
                {
                    bool loginFormat = loginInfo.LoginId.GetUntilOrEmpty().Equals(loginInfo.LoginType.Substring(0, 2),
                                                StringComparison.InvariantCultureIgnoreCase);
                    if (!loginFormat)
                        throw new Exception("Login id is not in proper format.");
                }
                else
                    throw new ArgumentException("Login id or Login type is null.");

                String query = String.Empty;
                String returnStr = String.Empty;

                bool exist = false;
                List<LoginInfo> users = null;
                List<MySqlParameter> prm = new List<MySqlParameter>();
                switch (loginInfo.LoginType.ToLower())
                {
                    case "distributor":
                    case "wholesaler":
                    case "retailer":
                        users = new PersistentHelper().GetUser("email_id", loginInfo.EMailId);
                        if (users.Count > 0) exist = true;
                        else
                        {
                            query = "INSERT INTO `mlo`.`users`(login_id,password,login_type,first_name,surname,firm_name,address," +
                                                             "gst_no,mobile_no,email_id,gender,profile_pic,dob,access_token,shop_no)" +
                                                       "VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?);Select LAST_INSERT_ID() as id;";
                            prm = new List<MySqlParameter>()
                            {
                                new MySqlParameter("@login_id", MySqlDbType.VarChar) {Value = loginInfo.LoginId},
                                new MySqlParameter("@password", MySqlDbType.VarChar) {Value = loginInfo.Password},
                                new MySqlParameter("@login_type", MySqlDbType.VarChar) {Value = loginInfo.LoginType},
                                new MySqlParameter("@first_name", MySqlDbType.VarChar) {Value = loginInfo.Name},
                                new MySqlParameter("@surname", MySqlDbType.VarChar) {Value = loginInfo.last_name},
                                new MySqlParameter("@firm_name", MySqlDbType.VarChar) {Value = loginInfo.firm_name},
                                new MySqlParameter("@address", MySqlDbType.VarChar) {Value = loginInfo.FirmAddress},
                                new MySqlParameter("@gst_no", MySqlDbType.VarChar) {Value = loginInfo.GSTNo},
                                new MySqlParameter("@mobile_no", MySqlDbType.VarChar) {Value = loginInfo.MobileNumber},
                                new MySqlParameter("@email_id", MySqlDbType.VarChar) {Value = loginInfo.EMailId},
                                new MySqlParameter("@gender", MySqlDbType.VarChar) {Value = loginInfo.Gender},
                                new MySqlParameter("@profile_pic", MySqlDbType.VarChar) {Value = loginInfo.Url},
                                new MySqlParameter("@dob", MySqlDbType.DateTime) {Value = new DateTime()},
                                new MySqlParameter("@access_token", MySqlDbType.VarChar) {Value = loginInfo.AccessToken},
                                new MySqlParameter("@shop_no", MySqlDbType.Int16) {Value = loginInfo.ShopNo}
                            };
                        }

                        break;

                    case "collector":
                        users = new PersistentHelper().GetCollector("email_id", loginInfo.EMailId);
                        if (users.Count > 0) exist = true;
                        else
                        {
                            query = "INSERT INTO `mlo`.`collector`(login_id, password, first_name, surname, address, mobile_no, " +
                                                                   "email_id, gender, profile_pic, dob, access_token, govt_id,  govt_id_no)" +
                                                         "VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?);" +
                                     "Select LAST_INSERT_ID() as id;";
                            prm = new List<MySqlParameter>()
                                {
                                    new MySqlParameter("@login_id", MySqlDbType.VarChar) {Value = loginInfo.LoginId},
                                    new MySqlParameter("@password", MySqlDbType.VarChar) {Value = loginInfo.Password},
                                    new MySqlParameter("@first_name", MySqlDbType.VarChar) {Value = loginInfo.Name},
                                    new MySqlParameter("@surname", MySqlDbType.VarChar) {Value = loginInfo.last_name},
                                    new MySqlParameter("@address", MySqlDbType.VarChar) {Value = loginInfo.FirmAddress},
                                    new MySqlParameter("@mobile_no", MySqlDbType.VarChar) {Value = loginInfo.MobileNumber},
                                    new MySqlParameter("@email_id", MySqlDbType.VarChar) {Value = loginInfo.EMailId},
                                    new MySqlParameter("@gender", MySqlDbType.VarChar) {Value = loginInfo.Gender},
                                    new MySqlParameter("@profile_pic", MySqlDbType.VarChar) {Value = loginInfo.Url},
                                    new MySqlParameter("@dob", MySqlDbType.DateTime) {Value = new DateTime()},
                                    new MySqlParameter("@access_token", MySqlDbType.VarChar) {Value = loginInfo.AccessToken},
                                    new MySqlParameter("@govt_id", MySqlDbType.VarChar) {Value = loginInfo.GovtId},
                                    new MySqlParameter("@govt_id_no", MySqlDbType.VarChar) {Value = loginInfo.GovtIdNo}
                                };
                        }

                        break;

                    case "loader":
                        users = new PersistentHelper().GetLoader("email_id", loginInfo.EMailId);
                        if (users.Count > 0) exist = true;
                        else
                        {
                            query = "INSERT INTO `mlo`.`loader`(login_id, password, first_name, surname, address, mobile_no, " +
                                                                 "email_id, gender, profile_pic, dob, access_token, govt_id, govt_id_no )" +
                                                        "VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?);" +
                                     "Select LAST_INSERT_ID() as id;";

                            prm = new List<MySqlParameter>()
                                {
                                    new MySqlParameter("@login_id", MySqlDbType.VarChar) {Value = loginInfo.LoginId},
                                    new MySqlParameter("@password", MySqlDbType.VarChar) {Value = loginInfo.Password},
                                    new MySqlParameter("@first_name", MySqlDbType.VarChar) {Value = loginInfo.Name},
                                    new MySqlParameter("@surname", MySqlDbType.VarChar) {Value = loginInfo.last_name},
                                    new MySqlParameter("@address", MySqlDbType.VarChar) {Value = loginInfo.FirmAddress},
                                    new MySqlParameter("@mobile_no", MySqlDbType.VarChar) {Value = loginInfo.MobileNumber},
                                    new MySqlParameter("@email_id", MySqlDbType.VarChar) {Value = loginInfo.EMailId},
                                    new MySqlParameter("@gender", MySqlDbType.VarChar) {Value = loginInfo.Gender},
                                    new MySqlParameter("@profile_pic", MySqlDbType.VarChar) {Value = loginInfo.Url},
                                    new MySqlParameter("@dob", MySqlDbType.DateTime) {Value = new DateTime()},
                                    new MySqlParameter("@access_token", MySqlDbType.VarChar) {Value = loginInfo.AccessToken},
                                    new MySqlParameter("@govt_id", MySqlDbType.VarChar) {Value = loginInfo.GovtId},
                                    new MySqlParameter("@govt_id_no", MySqlDbType.VarChar) {Value = loginInfo.GovtIdNo}
                                };
                        }

                        break;

                    case "driver":
                        List<DriverDetails> drivers = new PersistentHelper().GetDriver("email_id", loginInfo.EMailId);
                        if (drivers.Count > 0) exist = true;
                        else
                        {
                            query = "INSERT INTO `mlo`.`driver`(login_id, password, first_name, surname, address, mobile_no, " +
                                                                "email_id, gender, profile_pic, dob, access_token, govt_id, govt_id_no )" +
                                                       "VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?);" +
                                    "Select LAST_INSERT_ID() as id;";

                            prm = new List<MySqlParameter>()
                                {
                                    new MySqlParameter("@login_id", MySqlDbType.VarChar) {Value = loginInfo.LoginId},
                                    new MySqlParameter("@password", MySqlDbType.VarChar) {Value = loginInfo.Password},
                                    new MySqlParameter("@first_name", MySqlDbType.VarChar) {Value = loginInfo.Name},
                                    new MySqlParameter("@surname", MySqlDbType.VarChar) {Value = loginInfo.last_name},
                                    new MySqlParameter("@address", MySqlDbType.VarChar) {Value = loginInfo.FirmAddress},
                                    new MySqlParameter("@mobile_no", MySqlDbType.VarChar) {Value = loginInfo.MobileNumber},
                                    new MySqlParameter("@email_id", MySqlDbType.VarChar) {Value = loginInfo.EMailId},
                                    new MySqlParameter("@gender", MySqlDbType.VarChar) {Value = loginInfo.Gender},
                                    new MySqlParameter("@profile_pic", MySqlDbType.VarChar) {Value = loginInfo.Url},
                                    new MySqlParameter("@dob", MySqlDbType.DateTime) {Value = new DateTime()},
                                    new MySqlParameter("@access_token", MySqlDbType.VarChar) {Value = loginInfo.AccessToken},
                                    new MySqlParameter("@govt_id", MySqlDbType.VarChar) {Value = loginInfo.GovtId},
                                    new MySqlParameter("@govt_id_no", MySqlDbType.VarChar) {Value = loginInfo.GovtIdNo}
                                };
                        }

                        break;
                    default:
                        throw new Exception("Login type has not been specified.");
                }
                if (!exist)
                    returnStr = new PersistentHelper().SignUp(loginInfo, query, prm);
                else
                    returnStr = "Already Signed up with this email id.";

                return returnStr;
            }
            else
                throw new ArgumentException("Input is null!");
        }

        [HttpPost]
        public string AvailSubscription([FromBody] object json)
        {
            JObject jObject = JObject.Parse(json.ToString());
            string userId = (string)jObject["userId"];
            string subsId = (string)jObject["subsId"];
            string date = (string)jObject["date"];

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(subsId) || string.IsNullOrEmpty(date))
            {
                throw new Exception("Id or state cannot be null/empty");
            }

            return new PersistentHelper().AvailSubscription(userId, subsId, date);
        }
    }
}
