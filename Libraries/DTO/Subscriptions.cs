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
    public class Subscriptions
    {
        [DataMember(Name = "id")]
        public string id { get; set; }

        [DataMember(Name = "subs_id")]
        public string subsId { get; set; }

        [DataMember(Name = "display")]
        public string displayName { get; set; }

        [DataMember(Name = "min_del")]
        public string minDeliveries { get; set; }

        [DataMember(Name = "min_amt")]
        public string minAmount { get; set; }

        [DataMember(Name = "desc")]
        public string description { get; set; }

        [DataMember(Name = "cost_per_del")]
        public string costPerDelivery { get; set; }

        [DataMember(Name = "active")]
        public string active { get; set; }
    }
}
