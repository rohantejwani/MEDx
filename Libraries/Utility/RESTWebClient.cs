using log4net.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class RESTWebClient
    {

        #region Variable Declaration

        private WebClient m_client = null;
        private HttpWebRequest m_webRequestClient = null;
        #endregion

        //Added by Platform team for Workflow

        private Dictionary<string, string> customHeaders { get; set; }
        public RESTWebClient()
        {
            //Below signature added by Platform Team
            CreateWebClient();
        }


        private void CreateWebClient()
        {
            m_client = new WebClient();
            m_client.Headers = GetBasicHeaderCollection();
            m_client.Encoding = System.Text.Encoding.UTF8;
        }

        private WebHeaderCollection GetBasicHeaderCollection()
        {
            WebHeaderCollection headerCollection = new WebHeaderCollection();
            headerCollection[HttpRequestHeader.ContentType] = "application/json";
            //headerCollection[HttpRequestHeader.Date] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return headerCollection;
        }

        //public string ConvertObjectToJson(object objectToSerialize)
        //{
        //    string retVal = string.Empty;
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        DataContractJsonSerializer serializer = new DataContractJsonSerializer(objectToSerialize.GetType());
        //        serializer.WriteObject(ms, objectToSerialize);
        //        ms.Position = 0;

        //        using (StreamReader reader = new StreamReader(ms))
        //        {
        //            retVal = reader.ReadToEnd();
        //        }
        //    }

        //    return retVal;
        //}

        public string Post<T>(string url, object postData)
        {

            string result = "";
            try
            {
                string jsonStr = string.Empty;
                JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
                if (postData != null)
                    jsonStr = JsonConvert.SerializeObject(postData, microsoftDateFormatSettings);
                ////Logger.Log(Level.Info, "Url:" + url);
                ////Logger.Log(Level.Info, "Json String:" + jsonStr);
                result = m_client.UploadString(new Uri(url, UriKind.RelativeOrAbsolute), "POST", jsonStr);

            }
            catch (Exception ex)
            {
                //Logger.Log(Level.Error, "Error in invoking :: " + url + " \n Caused by :: " + ex);
                throw ex;
            }
            return result;

        }

        public string Get(string url, out bool success)
        {
            string result = null;
            try
            {
                //Logger.Log(Level.Debug, "URL :: " + url);
                result = m_client.DownloadString(new Uri(url, UriKind.RelativeOrAbsolute));
                //Logger.Log(Level.Debug, "Response :: " + result);
                success = true;
            }
            catch (Exception ex)
            {
                //Logger.Log(Level.Error, "Error in invoking :: " + url + " \n Caused by :: " + ex);
                success = false;
            }
            return result;
        }

    }


}
