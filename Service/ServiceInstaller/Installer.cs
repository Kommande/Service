using System.ComponentModel;
using System.ServiceProcess;
using System.Configuration.Install;
namespace ServiceInstaller
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        private System.ServiceProcess.ServiceProcessInstaller _serviceInstaller;
        private System.ServiceProcess.ServiceInstaller _processInstaller;

        public Installer()
        {
            InitializeComponent();

            _serviceInstaller = new ServiceProcessInstaller();
            _processInstaller = new System.ServiceProcess.ServiceInstaller();

            _serviceInstaller.Account = ServiceAccount.LocalSystem;
            _processInstaller.StartType = ServiceStartMode.Manual;
            _processInstaller.ServiceName = "Info Collector Service";
            Installers.Add(_processInstaller);
            Installers.Add(_serviceInstaller);
        }
    }
}
