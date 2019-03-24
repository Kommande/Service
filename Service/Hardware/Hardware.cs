using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardware
{
    class Hardware
    {
        public string OsVersion { get; set; }
        public bool Os64 { get; set; }
        public string PcName { get; set; }
        public int AmountOfCpus { get; set; }
        public List<string> LogicalDrives { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
