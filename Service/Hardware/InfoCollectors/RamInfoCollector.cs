using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Hardware
{
    public class RamInfoCollector
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static List<Ram> CollectInfo()
        {
            ManagementClass ram = new ManagementClass("Win32_PhysicalMemory");
            var rams = new List<Ram>();
            foreach (var t in ram.GetInstances())
            {
                try
                {
                    var speed = t.Properties["Speed"].Value.ToString();
                    var serialNumber = t.Properties["SerialNumber"].Value.ToString();
                    rams.Add(new Ram()
                    {
                        volume = (Double.Parse(t.Properties["Capacity"].Value.ToString()) / 1024 / 1024 / 1024).ToString(),
                        technicalSpeed = speed,
                        serialNumber = serialNumber
                    }
                        );
                }
                catch(Exception e)
                {
                    log.Error(e.Message);
                    log.Error(e.StackTrace);
                }
            }
            return rams;
        }
    }
}
