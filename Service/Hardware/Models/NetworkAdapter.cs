using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Hardware.Models
{
    public class NetworkAdapter
    {
        [DataMember]
        public string mac { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string speed { get; set; }
        [DataMember]
        public string [] ipAdresses { get; set; }
    }
}
