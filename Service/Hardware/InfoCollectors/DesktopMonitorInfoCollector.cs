using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Hardware
{
    public class DesktopMonitorInfoCollector
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
                    var monitorHeightInches = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height / GetDpi();
                    var monitorWidthInches = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width / GetDpi();
                    var model = monitor.Properties["Name"].Value.ToString() ?? "null";
                    var diagonal = Math.Sqrt(Math.Pow(monitorHeightInches, 2) + Math.Pow(monitorWidthInches, 2));
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

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public enum DeviceCap
        {
            Logpixelsy = 90,
        }
        public static int GetDpi()
        {
            var g = Graphics.FromHwnd(IntPtr.Zero);
            var desktop = g.GetHdc();
            var dpi = GetDeviceCaps(desktop, (int)DeviceCap.Logpixelsy);
            g.ReleaseHdc(desktop);
            return dpi;
        } 
    }
}
