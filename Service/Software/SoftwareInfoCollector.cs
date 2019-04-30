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
using Models;
using Newtonsoft.Json;

namespace Software
{
    public class SoftwareInfoCollector
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static List<Programm> CollectInfo(List<string> programms)
        {
            var pathList = programms;
            var programmsList = new List<Programm>();

            foreach (var path in pathList)
            {
                try
                {
                    Console.WriteLine(path);
                    var info = FileVersionInfo.GetVersionInfo(path);
                    var test = new FileInfo(path);
                   
                    programmsList.Add(new Programm() {
                        name = info.ProductName,
                        currentVersion = info.ProductVersion,
                        installDate = test.CreationTime.Date.ToString("yyyy/MM/dd H:mm")});
                
                }
                catch(Exception e)
                {
                    Console.WriteLine(string.Format("Enable to find a file: ",path));
                    Console.WriteLine(e.StackTrace);
                    log.Error(e.Message);
                    log.Error(e.StackTrace);
                }
            }
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";

            return programmsList;
           
        }

        private void NamedPipes(List<Item> data)
        {
            Task.Factory.StartNew(() =>
            {
                var server = new NamedPipeServerStream("InfoServicePipe");
                server.WaitForConnection();
                StreamReader reader = new StreamReader(server);
                StreamWriter writer = new StreamWriter(server);


                var obj = JsonConvert.SerializeObject(data);
                    writer.WriteLine(obj);
                    writer.Flush();
            });
        }
        public List<string> GetProgrammPath(List<string> programms)
        {
            var pathList = new List<string>();
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
            string[] skeys = key.GetSubKeyNames();
            int length = skeys.Length;
          
            for (int i = 0; i < length; i++)
            {
                RegistryKey appKey = key.OpenSubKey(skeys[i]);
                string name;
                var path = "";
                var date = "";
                try 
                {
                    name = appKey.GetValue("DisplayName").ToString();
                    path = appKey.GetValue("InstallLocation").ToString();
                    if (programms.Contains(name))
                    {
                        Console.WriteLine(name);
                        Console.WriteLine("path: " + path);
                        pathList.Add(path);
                    } 
                    //date = appKey.GetValue("InstallSource").ToString();
                }
                catch (Exception)
                {
                    continue;
                }
                /*
                Console.WriteLine(name);
                Console.WriteLine("path: " + path);
                Console.WriteLine("InstallSource: " + date);
                */
                appKey.Close();
            }
            key.Close();
            return pathList;
        }
        public IDictionary<string,string> GetProgrammPath()
        {
            var path64 = GetInstalledProgramsFromRegistry(RegistryView.Registry64);//Get64ProgramPath();
            Console.WriteLine(path64.Count);
            var path32 = GetInstalledProgramsFromRegistry(RegistryView.Registry32);//Get32ProgrammPath();
            Console.WriteLine(path32.Count);
            // var path = path64.Concat(path32.Where(x=>!path64.Select(y=>y.Key).Any(z=> x.Key==z))).ToDictionary(x=>x.Key,x=>x.Value);
            var path = path64.Concat(path32.Except(path64)).ToDictionary(x => x.Key, x => x.Value);
            foreach(var p in path)
            {
                Console.WriteLine("Name " + p.Key);
                Console.WriteLine("Path " + p.Value);
            }
            Console.WriteLine(path.Count);
            return path;
        }

        private IDictionary<string,string> Get64ProgramPath()
        {
            var pathList = new Dictionary<string,string>();
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
            string[] skeys = key.GetSubKeyNames();
            int length = skeys.Length;

            for (int i = 0; i < length; i++)
            {
                RegistryKey appKey = key.OpenSubKey(skeys[i]);
                string name;
                var path = "";
                var date = "";
                try
                {
                    name = appKey.GetValue("DisplayName").ToString();
                    path = appKey.GetValue("InstallLocation").ToString();
                    if (path.Count() != 0)
                   
                    {
                       
                        pathList.Add(name,path);
                    }
                  
                }
                catch (Exception)
                {
                    continue;
                }
               
                appKey.Close();
            }
            key.Close();
            return pathList;
        }

        private IDictionary<string,string> Get32ProgrammPath()
        {

            var pathList = new Dictionary<string, string>();
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
            string[] skeys = key.GetSubKeyNames();
            int length = skeys.Length;

            for (int i = 0; i < length; i++)
            {
                RegistryKey appKey = key.OpenSubKey(skeys[i]);
                string name;
                var path = "";
                var date = "";
                try
                {
                    name = appKey.GetValue("DisplayName").ToString();
                    path = appKey.GetValue("InstallLocation").ToString();
                    if (path.Count() != 0)
                    //if (name.Contains("Torrent"))
                    {
                      /*  Console.WriteLine(name);
                        Console.WriteLine("path: " + path);*/
                        pathList.Add(name, path);
                    }
                    //date = appKey.GetValue("InstallSource").ToString();
                }
                catch (Exception)
                {
                    continue;
                }
                /*
                Console.WriteLine(name);
                Console.WriteLine("path: " + path);
                Console.WriteLine("InstallSource: " + date);
                */
                appKey.Close();
            }
            key.Close();
            return pathList;
        }

        private static Dictionary<string,string> GetInstalledProgramsFromRegistry(RegistryView registryView)
        {
            var result = new Dictionary<string,string>();
            string name;
            var path = "";
            using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView).OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        if (IsProgramVisible(subkey))
                        {
                            name = subkey.GetValue("DisplayName").ToString();
                            path = subkey.GetValue("InstallLocation")?.ToString();
                            if (!result.Keys.Contains(name) &&path!=null &&path.Count()!=0)
                            {
                                result.Add(name, path);
                            }
                           
                        }
                    }
                }
            }

            return result;
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
