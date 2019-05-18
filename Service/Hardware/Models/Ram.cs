using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Runtime.Serialization;

namespace Hardware
{
    [DataContract]
    public class Ram
    {
        [DataMember]
        public string volume { get; set; }
        [DataMember]
        public string model { get; set; }
        [DataMember]
        public string technicalSpeed { get; set; }
        [DataMember]
        public string serialNumber { get; set; }
        
        public Ram()
        {

        }
        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            return info.ToString();
        }
    }
}
