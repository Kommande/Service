using Hardware.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Hardware.InfoCollectors
{
    public class NetworkAdapterInfoCollectorService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static List<NetworkAdapter> CollectInfo()
        {
            var networkAdapterConfigurationSearcher = 
                new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
            var networkAdapterSearcher = new ManagementObjectSearcher($"SELECT * FROM Win32_NetworkAdapter");
            var networkAdapterConfigurationInfos = networkAdapterConfigurationSearcher.Get().CollectionToList();
            var networkAdapterInfos = networkAdapterSearcher.Get().CollectionToList();

            var networkAdapters = new List<NetworkAdapter>();
            foreach (var networkAdapter in networkAdapterConfigurationInfos)
            {
                try
                {
                    networkAdapters
                        .Add(new NetworkAdapter
                        {
                            name = networkAdapter["Caption"]?.ToString(),
                            mac = networkAdapter["MACAddress"]?.ToString(),
                            speed = networkAdapterInfos.First(x=>x["Caption"].Equals(networkAdapter["Caption"]?
                                .ToString()))["Speed"]?.ToString(),
                            ipAdresses = (string[])networkAdapter["IPAddress"]
                        });
                }
                catch(Exception e)
                {
                    log.Error(e.Message);
                    log.Error(e.StackTrace);
                }

            }
            return networkAdapters;
        }
    }

    public static class Helpers
    {
        public static List<ManagementObject> CollectionToList(this System.Collections.ICollection collection)
        {
            var result = new List<ManagementObject>();
            result.AddRange(collection.Cast<ManagementObject>());
            return result;
        }
    }
   
}
