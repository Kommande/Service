using System;
using System.Configuration;
using Models.IpFilter;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            var ip1 = ConfigurationManager.AppSettings["IpV4Filter1"] == string.Empty ? null : ConfigurationManager.AppSettings["IpV4Filter1"];
            var ip2 = ConfigurationManager.AppSettings["IpV4Filter2"] == string.Empty ? null : ConfigurationManager.AppSettings["IpV4Filter2"];
            var ip3 = ConfigurationManager.AppSettings["IpV4Filter3"] == string.Empty ? null : ConfigurationManager.AppSettings["IpV4Filter3"];
            var ip4 = ConfigurationManager.AppSettings["IpV4Filter4"] == string.Empty ? null : ConfigurationManager.AppSettings["IpV4Filter4"];
            var port = ConfigurationManager.AppSettings["IpV4Port"] == string.Empty ? "6666" : ConfigurationManager.AppSettings["IpV4Port"];
            var pluginPath = ConfigurationManager.AppSettings["PluginPath"] == string.Empty ? null : ConfigurationManager.AppSettings["PluginPath"];
            var msiFilePath = ConfigurationManager.AppSettings["MsiFilePath"] == string.Empty ? null : ConfigurationManager.AppSettings["MsiFilePath"];
            var network = new Network.Network(new IpV4Filter() { Ip1 = ip1, Ip2 = ip2, Ip3 = ip3, Ip4 = ip4 },
                new Models.Configs.HttpServerConfigs() { PluginPath = pluginPath,MsiFilePath = msiFilePath,Port = port });
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
