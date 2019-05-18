using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardware
{
    public class Processor
    {
        public int amountOfCores { get; set; }
        public double cpu { get; set; }
        public bool virtualizationFirmwareEnabled { get; set; }
    }
}
