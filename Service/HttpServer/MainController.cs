using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Hardware;
using Network;
using Newtonsoft.Json;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.Configuration;
using System.Collections.Specialized;
using Models.Configs;
using Models;

namespace HttpServer
{
    public class MainController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public HttpServerConfigs configs { get; internal set; }
        public MainController(HttpServerConfigs configs)
        {
            this.configs = configs;
        }
        public ServiceActionResult InitDowload(HttpListenerRequest request)
        {
            try
            {
                new Thread(() =>
                {
                    var query = HttpUtility.ParseQueryString(request.Url.Query);
                    var url = query.Get(0);
                    var fileName = query.Get(1);
                    fileName = fileName.Remove(0, 1);
                    fileName = fileName.Remove(fileName.Length - 1, 1);
                    WebClient myWebClient = new WebClient();
                    var path = AppDomain.CurrentDomain.BaseDirectory + @"\";// @"D:\Диплом\";
                    string myStringWebResource = url + fileName;
                    Console.WriteLine("Downloading File \"{0}\" from \"{1}\" .......\n\n", fileName, myStringWebResource);
                    myWebClient.DownloadFile(myStringWebResource, path + fileName);
                    Console.WriteLine("Successfully Downloaded File \"{0}\" from \"{1}\"", fileName, myStringWebResource);
                    Install(AppDomain.CurrentDomain.BaseDirectory + @"\" + fileName);
                }).Start();

                return new ServiceActionResult() { Result = true, Message = "Download started", HttpResponseCode = 200 };
                //return "Download started";
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                log.Error(e.StackTrace);
                return new ServiceActionResult() { Result = true, Message = e.Message, HttpResponseCode = 500 };
            }

        }

        private bool Install(string filePath)
        {

            var installerFilePath = filePath;
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = installerFilePath;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.Arguments = "/silent";
            // var installerProcess = System.Diagnostics.Process.Start(installerFilePath, "/silent",);
            var installerProcess = System.Diagnostics.Process.Start(startInfo);
            //installerProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            //installerProcess.a
            installerProcess.WaitForExit();
            return true;
        }
        public ServiceActionResult GetMainInfo(HttpListenerRequest request)
        {
            var query = HttpUtility.ParseQueryString(request.Url.Query,Encoding.UTF8);
            Console.WriteLine("query: "+ query);
            var programmString = query.Get(0);//network.AcceptCommand();
            Console.WriteLine("programmString: " + programmString);
            programmString = programmString.Remove(0, 3);
            programmString = programmString.Remove(programmString.Length - 3, 3);
            Console.WriteLine("programmString: " + programmString);
            programmString = programmString.Replace("%5C","\\");
            programmString = programmString.Replace("%20", " ");
            Console.WriteLine("programmString: " + programmString);
            var commandParser = new CommandParser();
            // var pathList = commandParser.ParsePath(programmString);
            var programmsInfo = Software.SoftwareInfoCollector.CollectInfo(programmString.Split(',').ToList());
            Console.WriteLine("Received");
            var hddSet = HddInfoCollector.CollectInfo();
            var ramSet = RamInfoCollector.CollectInfo();
            var monitor = DesctopMonitorInfoCollector.CollectInfo();
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
            return new ServiceActionResult() { Result = true, Message = resultString.ToString(), HttpResponseCode = 200 };
        }

        public ServiceActionResult InvokeCustomModule(HttpListenerRequest request)
        {
            try
            {
                var query = HttpUtility.ParseQueryString(request.Url.Query);
                var fileName = query.GetParametr(1);
                var pluginClass = query.GetParametr(2);
                var method = query.GetParametr(3);
                var paramets = query.GetParametr(4);
                Assembly plugin = Assembly.Load(configs.PluginPath + fileName);
                // Get the type to use.
                Type pluginType = plugin.GetType(pluginClass);
                // Get the method to call.
                MethodInfo pluginMethod = pluginType.GetMethod(method);
                // Create an instance.
                object obj = Activator.CreateInstance(pluginType);
                // Execute the method.
                var res = (string)pluginMethod.Invoke(obj, paramets.Split(','));
                return new ServiceActionResult() { Result = true, Message = res, HttpResponseCode = 200 };
            }
            catch(Exception e)
            {
                log.Error(e.Message);
                log.Error(e.StackTrace);
                return new ServiceActionResult() { Result = true, Message = e.Message, HttpResponseCode = 500 };
            }
            
        }
    }

    public static class HttpHelper
    {
        public static string GetParametr(this NameValueCollection col, int index)
        {
            var str =col.Get(index).Remove(0, 1);
            str = str.Remove(str.Length - 1, 1);
            return str;
        }
    }
}
