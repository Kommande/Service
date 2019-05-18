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
        public string Mac { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Speed { get; set; }
        [DataMember]
        public string [] IPAdresses { get; set; }
    }
}
