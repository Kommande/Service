using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Threading.Tasks;

namespace Hardware
{
    public class ProcessorInfoCollector
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static List<Processor> CollectInfo()
        {
            ManagementClass mc = new ManagementClass("win32_processor");
            var moc = mc.GetInstances();
            var cpus = new List<Processor>();
            foreach(var cpu in moc)
            {
                cpus.Add(new Processor()
                {
                    cpu = Double.Parse(cpu.Properties["MaxClockSpeed"].Value.ToString())
                    ,
                    amountOfCores = Int32.Parse(cpu.Properties["NumberOfCores"].Value.ToString())
                });
            }
            return cpus;
        }
        public static Processor CollectInfoFirstProcessor()
        {
            try
            {
                ManagementClass mc = new ManagementClass("win32_processor");
                var moc = mc.GetInstances();
                var enumerator = moc.GetEnumerator();
                enumerator.MoveNext();
                var cpu = enumerator.Current;
                return new Processor()
                {
                    cpu = Double.Parse(cpu.Properties["MaxClockSpeed"].Value.ToString()),
                    amountOfCores = Int32.Parse(cpu.Properties["NumberOfCores"].Value.ToString()),
                    virtualizationFirmwareEnabled = Boolean.Parse(cpu.Properties["VirtualizationFirmwareEnabled"].Value.ToString())
                };
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                log.Error(e.StackTrace);
                return new Processor();
            }
        }
    }
}
