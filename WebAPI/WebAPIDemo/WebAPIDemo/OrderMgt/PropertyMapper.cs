using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utility;

namespace WebAPIDemo.OrderMgt
{
    public class PropertyMapper
    {

        internal static List<PropertyMap> MapOrderForEmployee()
        {
            List<PropertyMap> listPropertyMap = new List<PropertyMap>();

            listPropertyMap.Add(new PropertyMap("id", "id"));
            listPropertyMap.Add(new PropertyMap("user_id", "uid"));
            listPropertyMap.Add(new PropertyMap("destination", "dest"));

            listPropertyMap.Add(new PropertyMap("loc_coordinates", "coord"));
            listPropertyMap.Add(new PropertyMap("challan_no", "challan"));
            listPropertyMap.Add(new PropertyMap("no_of_Items", "noi"));

            //listPropertyMap.Add(new PropertyMap("request_time", "rt"));
            listPropertyMap.Add(new PropertyMap("pickup_by", "pb"));
            //listPropertyMap.Add(new PropertyMap("pickup_time", "pt"));

            
            listPropertyMap.Add(new PropertyMap("vehicle_no", "vn"));
            listPropertyMap.Add(new PropertyMap("driver", "driver"));

            //listPropertyMap.Add(new PropertyMap("load_time", "lt"));
            listPropertyMap.Add(new PropertyMap("state", "state"));



            return listPropertyMap;
        }

        internal static List<PropertyMap> MapOrderForWholesaler()
        {
            List<PropertyMap> listPropertyMap = MapOrder();
            listPropertyMap.Add(new PropertyMap("wholesaler_name", "wn"));
            listPropertyMap.Add(new PropertyMap("firm_name", "firm_name"));
            listPropertyMap.Add(new PropertyMap("pickup_by_name", "pbn"));
            listPropertyMap.Add(new PropertyMap("active", "active"));


            return listPropertyMap;
        }

        internal static List<PropertyMap> MapOrderForUser()
        {
            List<PropertyMap> listPropertyMap = MapOrder();
            listPropertyMap.Add(new PropertyMap("wholesaler_name", "wn"));
            listPropertyMap.Add(new PropertyMap("firm_name", "firm_name"));
            listPropertyMap.Add(new PropertyMap("shop_no", "sn"));
            

            return listPropertyMap;
        }
        internal static List<PropertyMap> MapOrder()
        {
            List<PropertyMap> listPropertyMap = new List<PropertyMap>();

            listPropertyMap.Add(new PropertyMap("id", "id"));
            listPropertyMap.Add(new PropertyMap("user_id", "uid"));
           
            listPropertyMap.Add(new PropertyMap("destination", "dest"));

            listPropertyMap.Add(new PropertyMap("loc_coordinates", "coord"));
            listPropertyMap.Add(new PropertyMap("challan_no", "challan"));
            listPropertyMap.Add(new PropertyMap("no_of_Items", "noi"));

           // listPropertyMap.Add(new PropertyMap("request_time", "rt"));
            listPropertyMap.Add(new PropertyMap("pickup_by", "pb"));
            //listPropertyMap.Add(new PropertyMap("pickup_time", "pt"));

            //listPropertyMap.Add(new PropertyMap("drop_time", "dt"));
            listPropertyMap.Add(new PropertyMap("vehicle_no", "vn"));
            listPropertyMap.Add(new PropertyMap("driver", "driver"));

            //listPropertyMap.Add(new PropertyMap("load_time", "lt"));
            listPropertyMap.Add(new PropertyMap("state", "state"));

            return listPropertyMap;
        }
        internal static List<PropertyMap> MapOrderId()
        {
            List<PropertyMap> listPropertyMap = new List<PropertyMap>();
            listPropertyMap.Add(new PropertyMap("id", "id"));
            return listPropertyMap;
        }
        }
}