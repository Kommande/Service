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
            var serverUrl = ConfigurationManager.AppSettings["ServerUrl"] == string.Empty ? null : ConfigurationManager.AppSettings["ServerUrl"];
            var msiFileServerUrl = ConfigurationManager.AppSettings["MsiFileServerUrl"] == string.Empty ? null : ConfigurationManager.AppSettings["MsiFileServerUrl"];
            var dllFileServerUrl = ConfigurationManager.AppSettings["DllFileServerUrl"] == string.Empty ? null : ConfigurationManager.AppSettings["DllFileServerUrl"];
            var ConfgiFileServerUrl = ConfigurationManager.AppSettings["ConfigFileServerUrl"] == string.Empty ? null : ConfigurationManager.AppSettings["ConfigFileServerUrl"];
            //ConfigFileServerUrl
            var network = new Network.Network(new IpV4Filter() { Ip1 = ip1, Ip2 = ip2, Ip3 = ip3, Ip4 = ip4 },
                new Models.Configs.HttpServerConfigs()
                { PluginPath = pluginPath,
                    MsiFilePath = msiFilePath,
                    Port = port,
                    ServerUrl = serverUrl,
                    MsiFileServerUrl = msiFilePath,
                    DllFileServerUrl = dllFileServerUrl,
                    ConfigFileServerUrl = ConfgiFileServerUrl
                });
            network.StartHttpServer();
          
            Console.ReadLine();

        }

}
}
