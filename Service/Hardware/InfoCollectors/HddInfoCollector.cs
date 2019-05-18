using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardware
{
    public class HddInfoCollector
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static List<Hdd> CollectInfo()
        {
            var volume = "";
            var fragmentation = "";
            var hdds = new List<Hdd>();
            foreach (var drive in DriveInfo.GetDrives())
            {
                try
                {
                    if (drive.IsReady)
                    {
                        volume = drive.Name;
                        fragmentation = (drive.AvailableFreeSpace / 1024 / 1024 / 1024).ToString() + "/" + (drive.TotalSize / 1024 / 1024 / 1024).ToString();
                        hdds.Add(new Hdd() { diskName = volume, fragmentation = fragmentation, model = drive.DriveType.ToString() });
                    }
                }
                catch(Exception e)
                {
                    log.Error(e.Message);
                    log.Error(e.StackTrace);
                }
            }
            return hdds;
        }
    }
}
