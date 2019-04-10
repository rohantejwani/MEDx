using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    [DataContract]
    public class Location
    {

        [DataMember(Name = "id")]
        [Description("Location id")]
        public string id { get; set; }


        [DataMember(Name = "ln")]
        [Description("Location name")]
        public string name { get; set; }

        //[DataMember(Name = "coord")]
        //[Description("Location Coordinates")]
        //public string coordinates { get; set; }

        [DataMember(Name = "lat")]
        [Description("Latitude")]
        public string latitude { get; set; }

        [DataMember(Name = "lng")]
        [Description("Longitude")]
        public string longitude { get; set; }

        [DataMember(Name = "address")]
        [Description("Address")]
        public string address { get; set; }

        [DataMember(Name = "dlat")]
        [Description("Depot Latitude")]
        public string dLatitude { get; set; }

        [DataMember(Name = "dlng")]
        [Description("Depot Longitude")]
        public string dLongitude { get; set; }

        [DataMember(Name = "did")]
        [Description("Depot Id")]
        public string dId { get; set; }

        [DataMember(Name = "updated")]
        [Description("Depot Updated")]
        public string updated { get; set; }

    }
}
