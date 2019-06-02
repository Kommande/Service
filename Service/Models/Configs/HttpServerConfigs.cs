using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Configs
{
    public class HttpServerConfigs
    {
        public string Port { get; set; }
        public string PluginPath { get; set; }
        public string MsiFilePath { get; set; }
        public string ServerUrl { get; set; }
        public string MsiFileServerUrl { get; set; }
        public string DllFileServerUrl { get; set; }
        public string ConfigFileServerUrl { get; set; }
    }
}
