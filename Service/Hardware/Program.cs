using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardware
{
    class Program
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            Console.WriteLine("Версия Windows: {0}",
               Environment.OSVersion);
            Console.WriteLine("64 Bit операционная система? : {0}",
               Environment.Is64BitOperatingSystem ? "Да" : "Нет");
            Console.WriteLine("Имя компьютера : {0}",
               Environment.MachineName);
            Console.WriteLine("Число процессоров : {0}",
               Environment.ProcessorCount);
            Console.WriteLine("Системная папка : {0}",
               Environment.SystemDirectory);
            Console.WriteLine("Логические диски : {0}",
                  String.Join(", ", Environment.GetLogicalDrives())
               .TrimEnd(',', ' ')
               .Replace("\\", String.Empty));
            //new Hdd().CollectInfo();
           // new Ram().CollectInfo();
            Console.ReadKey();
        }
    }
}
