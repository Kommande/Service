using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class InstallService: IInstallService
    {
        public bool Install(string filePath)
        {
            Console.WriteLine("filePath: " + filePath);

            Process installerProcess = new Process();
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.Arguments = string.Format("/i {0} /q REINSTALL = ALL REAINSTALLMODE=omus", filePath);
            processInfo.FileName = "msiexec";
            installerProcess.StartInfo = processInfo;
            installerProcess.Start();
            installerProcess.WaitForExit();
            return true;
        }
    }
}
