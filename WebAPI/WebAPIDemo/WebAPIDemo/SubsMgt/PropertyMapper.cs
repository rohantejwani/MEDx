using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utility;

namespace WebAPIDemo.SubsMgt
{
    public class PropertyMapper
    {
        internal static List<PropertyMap> MapSubscriptions()
        {
            List<PropertyMap> listPropertyMap = new List<PropertyMap>();

            listPropertyMap.Add(new PropertyMap("id", "id"));
            listPropertyMap.Add(new PropertyMap("subs_id", "subs_id"));
            listPropertyMap.Add(new PropertyMap("display_name", "display"));
            listPropertyMap.Add(new PropertyMap("min_deliveries", "min_del"));
            listPropertyMap.Add(new PropertyMap("min_amount", "min_amt"));
            listPropertyMap.Add(new PropertyMap("description", "desc"));
            listPropertyMap.Add(new PropertyMap("cost_per_delivery", "cost_per_del"));
            listPropertyMap.Add(new PropertyMap("active", "active"));
            
            return listPropertyMap;
        }
    }
}