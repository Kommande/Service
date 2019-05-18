using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Hardware
{
    public class DesctopMonitorInfoCollector
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static List<DesctopMonitor> CollectInfo()
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Service='monitor'");
            var monitorsInfo = searcher.Get();
            var monitors = new List<DesctopMonitor>();
            foreach (var monitor in monitorsInfo)
            {
                try
                {
                    var MonitorHeightInches = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height / 96;
                    var MonitorWidthInches = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width / 96;
                    var model = monitor.Properties["Name"].Value.ToString() ?? "null";
                    var diagonal = Math.Sqrt(Math.Pow(MonitorHeightInches, 2) + Math.Pow(MonitorWidthInches, 2));
                    monitors.Add(new DesctopMonitor() { model = model, diagonal = Math.Round(diagonal, 1).ToString() });
                }
                catch(Exception e)
                {
                    log.Error(e.Message);
                    log.Error(e.StackTrace);
                }
            }
            return monitors;
        }
    }
}
