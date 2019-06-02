using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.Runtime.CompilerServices;
using Models;
using Newtonsoft.Json;
using System.Globalization;

namespace Software
{
    public class SoftwareInfoCollector
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static List<Program> CollectInfo()
        {
            return GetProgramList();
        }
        public static List<Program> CollectInfo(List<string> programs)
        {
            var pathList = programs;
            var programsList = new List<Program>();
            foreach (var path in pathList)
            {
                try
                {
                    Console.WriteLine(path);
                    var info = FileVersionInfo.GetVersionInfo(path);
                    var test = new FileInfo(path);

                    programsList.Add(new Program()
                    {
                        name = info.ProductName,
                        currentVersion = info.ProductVersion,
                        installDate = test.CreationTime.Date.ToString("yyyy/MM/dd H:mm")
                    });

                }
                catch (Exception e)
                {
                    Console.WriteLine(string.Format("Enable to find a file: ", path));
                    Console.WriteLine(e.StackTrace);
                    log.Error(e.Message);
                    log.Error(e.StackTrace);
                }
            }
            return programsList;
        }
        private static List<Program> GetProgramList()
        {
            var path64 = GetInstalledProgramsFromRegistry(RegistryView.Registry64);
            Console.WriteLine(path64.Count);
            var path32 = GetInstalledProgramsFromRegistry(RegistryView.Registry32);
            Console.WriteLine(path32.Count);
            var path = path64.Concat(path32.Except(path64));
            return path.ToList();
        }

        private static List<Program> GetInstalledProgramsFromRegistry(RegistryView registryView)
        {
            var result = new List<string>();
            var programResult = new List<Program>();
            string name = null;
            string installDate = null;
            string version = null;
            using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView).OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        if (IsProgramVisible(subkey))
                        {
                            name = subkey.GetValue("DisplayName")?.ToString().Split('\\').Last();
                            version = subkey.GetValue("DisplayVersion")==null? string.Empty : subkey.GetValue("DisplayVersion").ToString();
                            installDate =  subkey.GetValue("InstallDate")?.ToString();

                            DateTime date = new DateTime();
                            DateTime.TryParseExact(installDate, "yyyyMMdd", null, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AdjustToUniversal, out date);
                            installDate = date.ToString("yyyy.MM.dd H:mm");
                            //installDate = installDate.Replace('.', '/');

                            if (!result.Contains(name))
                            {
                                result.Add(name);
                                programResult.Add(new Program(){name = name,currentVersion = version
                                    ,installDate = installDate});
                            }
                           
                        }
                    }
                }
            }

            return programResult;
        }

        private static bool IsProgramVisible(RegistryKey subkey)
        {
            var name = (string)subkey.GetValue("DisplayName");
            var releaseType = (string)subkey.GetValue("ReleaseType");
            //var unistallString = (string)subkey.GetValue("UninstallString");
            var systemComponent = subkey.GetValue("SystemComponent");
            var parentName = (string)subkey.GetValue("ParentDisplayName");

            return
                !string.IsNullOrEmpty(name)
                && string.IsNullOrEmpty(releaseType)
                && string.IsNullOrEmpty(parentName)
                && (systemComponent == null);
        }
    }
}
