using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    [DataContract(Name = "SBMapper")]
    public class SBMapper
    {
        public SBMapper()
        {
        }

        public SBMapper(List<PropertyMap> mapperCollection)
        {
            MapperCollection = mapperCollection;
        }

        [DataMember(Name = "MapperCollection")]
        public List<PropertyMap> MapperCollection { get; set; }
    }

    [DataContract(Name = "PropertyMap")]
    public class PropertyMap
    {
        public PropertyMap() { }

        public PropertyMap(string sourceProperty, string destinationProperty)
        {
            SourceProperty = sourceProperty;
            DestinationProperty = destinationProperty;
        }

        [DataMember(Name = "SourceProperty")]
        public string SourceProperty { get; set; }
        [DataMember(Name = "DestinationProperty")]
        public string DestinationProperty { get; set; }

    }

}
