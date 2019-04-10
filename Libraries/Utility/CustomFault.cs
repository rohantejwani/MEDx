using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    [DataContract]
    public class CustomFault
    {
        public CustomFault(string faultMessage, string errorCode, string description)
        {
            FaultMessage = faultMessage;
            ErrorCode = errorCode;
            this.Description = description;
        }

        [DataMember]
        public string FaultMessage { get; set; }

        [DataMember]
        public string ErrorCode { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
