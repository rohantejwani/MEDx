using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    [DataContract]
    public class OrderDetails
    {
        [DataMember(Name = "id")]
        [Description("Order Id")]
        public string Id { get; set; }

        [DataMember(Name = "firm_name")]
        [Description("Firm Name")]
        public string FirmName { get; set; }

        [DataMember(Name = "wn")]
        [Description("Wholesaler Name")]
        public string WholesalerName { get; set; }

        [DataMember(Name = "sn")]
        [Description("Shop number")]
        public string ShopNo { get; set; }

        [DataMember(Name = "uid")]
        [Description("User Id")]
        public string userId { get; set; }

        [DataMember(Name = "dest")]
        [Description("destination")]
        public string destination { get; set; }

        [DataMember(Name = "coord")]
        [Description("Location Coordinates")]
        public string locationCoordinates { get; set; }

        [DataMember(Name = "locid")]
        [Description("Location Id")]
        public string locId { get; set; }


        [DataMember(Name = "lat")]
        [Description("latitude")]
        public string latitude { get; set; }

        [DataMember(Name = "lng")]
        [Description("longitude")]
        public string longitude { get; set; }


        [DataMember(Name = "challan")]
        [Description("Challan Number")]
        public string challanNumber { get; set; }

        [DataMember(Name = "noi")]
        [Description("Number of items")]
        public int noOfItems { get; set; }

        [DataMember(Name = "rt")]
        [Description("Request Time")]
        public DateTime? requestTime { get; set; }

        [DataMember(Name = "pb")]
        [Description("Pickup by")]
        public Int16? pickupBy { get; set; }

        [DataMember(Name = "pbn")]
        [Description("Pickup by name")]
        public string pickupByName { get; set; }

        [DataMember(Name = "pt")]
        [Description("Pickup Time")]
        public DateTime? pickupTime { get; set; }

        [DataMember(Name = "dt")]
        [Description("Drop Time")]
        public DateTime? dropTime { get; set; }

        [DataMember(Name = "vn")]
        [Description("Vehicle no")]
        public string vehicleNo { get; set; }

        [DataMember(Name = "driver")]
        [Description("Driver")]
        public int driver { get; set; }

        [DataMember(Name = "lt")]
        [Description("Load Time")]
        public DateTime? loadTime { get; set; }

        [DataMember(Name = "state")]
        [Description("State of order")]
        public string state { get; set; }

        [DataMember(Name = "active")]
        [Description("Active")]
        public string active { get; set; }

    }
}
