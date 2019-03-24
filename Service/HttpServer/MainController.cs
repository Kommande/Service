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

namespace HttpServer
{
    public class MainController
    {



        public string InitDowload(HttpListenerRequest request)
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


                return "Download started";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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
        public string GetMainInfo(HttpListenerRequest request)
        {
            var query = HttpUtility.ParseQueryString(request.Url.Query);
            var programmString = query.Get(0);//network.AcceptCommand();
            programmString = programmString.Remove(0, 1);
            programmString = programmString.Remove(programmString.Length - 1, 1);
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
            return resultString.ToString();
        }
    }
}
