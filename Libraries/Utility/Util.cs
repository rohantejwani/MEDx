using DTO;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class Util
    {
        public static string getHeaderValue(string header)
        {
            Logger.Log(Level.Debug, "GetHeaderValue for " + header);
            string request_header_value1 = "";
            try
            {
                Logger.Log(Level.Debug, "request_header_value : ");
                WebOperationContext ctx = WebOperationContext.Current;
                request_header_value1 = ctx.IncomingRequest.Headers[header].ToString();
            }
            catch (Exception ex)
            {
                Logger.Log(Level.Error, "Exception in getting header value : " + ex.StackTrace);
                //throw new WebFaultException<CustomFault>(
                //    new CustomFault("Request header '" + header + "' has not been specified", SkyboErrorCodes.BadRequest, ex.Message),
                //HttpStatusCode.BadRequest);
                throw ex;
            }
            return request_header_value1;
        }

        public void LoginIdGenerator(LoginInfo loginInfo)
        {
            if (!string.IsNullOrEmpty(loginInfo.LoginType))
            {
                int _min = 10000;
                int _max = 99999;
                Random _rdm = new Random();

                string str = loginInfo.LoginType.Substring(0, 2);
                if (string.IsNullOrEmpty(loginInfo.ShopNo))
                    loginInfo.LoginId = str + "_" + _rdm.Next(_min, _max);
                else
                    loginInfo.LoginId = str + "_" + loginInfo.ShopNo + "_" + _rdm.Next(_min, _max);
                Logger.Log(Level.Info, "Login Id ::" + loginInfo.LoginId);
            }
            else
            {
                Logger.Log(Level.Error, "Login type not defined.!");
                throw new Exception("Login type not defined.!");
            }
        }
    }
}
