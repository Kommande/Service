using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.IO;
using System.Runtime.Serialization;

namespace Hardware
{
    [DataContract]
    public class Hdd
    {
        [DataMember]
        public string fragmentation { get; set; }

        [DataMember]
        public string model { get; set; }
   
        [DataMember]
        public string diskName { get; set; }

        public Hdd()
        {

        }
    }
}
