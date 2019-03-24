using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardware;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Network;
using Newtonsoft.Json.Linq;
using System.Management;
using HttpServer;
using Software;
using System.Configuration;
using Models.IpFilter;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = ConfigurationManager.AppSettings["IpV4Filter1"];
            var ip1 = ConfigurationManager.AppSettings["IpV4Filter1"] == string.Empty ? null : ConfigurationManager.AppSettings["IpV4Filter1"];
            var ip2 = ConfigurationManager.AppSettings["IpV4Filter2"] == string.Empty ? null : ConfigurationManager.AppSettings["IpV4Filter2"];
            var ip3 = ConfigurationManager.AppSettings["IpV4Filter3"] == string.Empty ? null : ConfigurationManager.AppSettings["IpV4Filter3"];
            var ip4 = ConfigurationManager.AppSettings["IpV4Filter4"] == string.Empty ? null : ConfigurationManager.AppSettings["IpV4Filter4"];
            var network = new Network.Network(new IpV4Filter() { Ip1 = ip1, Ip2 = ip2, Ip3 = ip3, Ip4 = ip4 });
            network.StartHttpServer();
            /*var a = new SoftwareInfoCollector();

            a.GetProgrammPath();*/
            Console.ReadLine();
            /*while (true)
             {
                Console.WriteLine("Жду");
                var programmString = network.AcceptCommand();
                var commandParser = new CommandParser();
                var pathList = commandParser.ParsePath(programmString);
                var programmsInfo = Software.SoftwareInfoCollector.CollectInfo(pathList);
                Console.WriteLine("Получил");
                var hddSet = HddInfoCollector.CollectInfo();
                var ramSet = RamInfoCollector.CollectInfo();
                var monitor =  DesctopMonitorInfoCollector.CollectInfo();
                var processorSet = ProcessorInfoCollector.CollectInfoFirstProcessor();
                var resultString = new StringBuilder();
                resultString.Append("{");
                resultString.Append(string.Format("\"softwareSet\": {0}," + Environment.NewLine, JsonConvert.SerializeObject(programmsInfo)));
                resultString.Append("\"hardware\": {");
                resultString.Append(string.Format("\"processor\": {0}," + Environment.NewLine, JsonConvert.SerializeObject(processorSet)));
                resultString.Append(string.Format("\"hddSet\": {0}," + Environment.NewLine, JsonConvert.SerializeObject(hddSet)));
                resultString.Append(string.Format("\"ramSet\": {0}," + Environment.NewLine, JsonConvert.SerializeObject(ramSet)));
                resultString.Append(string.Format("\"monitor\": {0}" + Environment.NewLine, JsonConvert.SerializeObject(monitor.First())));
                resultString.Append("}");
                resultString.Append("}");
                Console.WriteLine(resultString);
                Console.WriteLine("Отправляю");
                network.SendResult(resultString.ToString());
                Console.WriteLine("Отправил");
        }*/
        }

}
}
