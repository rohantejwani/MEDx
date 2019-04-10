using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utility;

namespace WebAPIDemo.LocationMgt
{
    public class PropertyMapper
    {
        internal static List<PropertyMap> MapLocation()
        {
            List<PropertyMap> listPropertyMap = new List<PropertyMap>();
            listPropertyMap.Add(new PropertyMap("id", "id"));
            listPropertyMap.Add(new PropertyMap("name", "ln"));
            listPropertyMap.Add(new PropertyMap("latitude", "lat"));
            listPropertyMap.Add(new PropertyMap("longitude", "lng"));
            listPropertyMap.Add(new PropertyMap("address", "address"));
            listPropertyMap.Add(new PropertyMap("dlat", "dlat"));
            listPropertyMap.Add(new PropertyMap("dlng", "dlng"));
            listPropertyMap.Add(new PropertyMap("d_id", "did"));
            listPropertyMap.Add(new PropertyMap("updated", "updated"));
            return listPropertyMap;
        }
    }
}