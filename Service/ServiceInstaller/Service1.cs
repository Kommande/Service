using Models.IpFilter;
using Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ServiceInstaller
{
    public partial class Service1 : ServiceBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                log.Info("Service Started");

                var ip1 = ConfigurationManager.AppSettings["IpV4Filter1"] == string.Empty ? null : ConfigurationManager.AppSettings["IpV4Filter1"];
                var ip2 = ConfigurationManager.AppSettings["IpV4Filter2"] == string.Empty ? null : ConfigurationManager.AppSettings["IpV4Filter2"];
                var ip3 = ConfigurationManager.AppSettings["IpV4Filter3"] == string.Empty ? null : ConfigurationManager.AppSettings["IpV4Filter3"];
                var ip4 = ConfigurationManager.AppSettings["IpV4Filter4"] == string.Empty ? null : ConfigurationManager.AppSettings["IpV4Filter4"];
                var PluginPath = ConfigurationManager.AppSettings["PluginPath"];

                log.Info(string.Format("{0}.{1}.{2}.{3}", ip1, ip2, ip3, ip4));
                var network = new Network.Network(new IpV4Filter() { Ip1 = ip1, Ip2 = ip2, Ip3 = ip3, Ip4 = ip4 }, 
                    new Models.Configs.HttpServerConfigs() { PluginPath = PluginPath});
                network.StartHttpServer();
            }
            catch(Exception e)
            {
                log.Error(e.Message);
                log.Error(e.StackTrace);
            }

        }

        protected override void OnStop()
        {
            log.Info("Service Stoped");
        }
    }
}
