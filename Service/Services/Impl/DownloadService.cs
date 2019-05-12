using Models;
using Models.Configs;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Services
{
    public class DownloadService: IDownloadService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IInstallService installService;
        private readonly HttpServerConfigs serverConfigs;

        public DownloadService(IInstallService installService, HttpServerConfigs serverConfigs)
        {
            this.installService = installService;
            this.serverConfigs = serverConfigs;
        }
        public ServiceActionResult DownloadAndInstallMSI(HttpListenerRequest request)
        {
            try
            {
                new Thread(() =>
                {
                    var query = HttpUtility.ParseQueryString(request.Url.Query);
                    var url = query.Get(0);
                    var fileName = query.Get(1);
                    /*fileName = fileName.Remove(0, 1);
                    fileName = fileName.Remove(fileName.Length - 1, 1);*/
                    WebClient myWebClient = new WebClient();
                    var path = AppDomain.CurrentDomain.BaseDirectory;// @"D:\Диплом\";
                    string myStringWebResource = url;
                    var downloadInfo = string.Format("Downloading File \"{0}\" from \"{1}\" .......\n\n", fileName, myStringWebResource);
                    log.Info(downloadInfo);
                    Console.WriteLine(downloadInfo);
                    myWebClient.DownloadFile(myStringWebResource, path + fileName);
                    downloadInfo = string.Format("Successfully Downloaded File \"{0}\" from \"{1}\"", fileName, myStringWebResource);
                    log.Info(downloadInfo);
                    Console.WriteLine(downloadInfo);
                    this.installService.Install(path + fileName);
                }).Start();

                return new ServiceActionResult() { Result = true, Message = "true", HttpResponseCode = 200 };
                //return "Download started";
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                log.Error(e.StackTrace);
                return new ServiceActionResult() { Result = true, Message = e.Message, HttpResponseCode = 500 };
            }

        }

        public ServiceActionResult DownloadDll(HttpListenerRequest request)
        {
            try
            {
                new Thread(() =>
                {
                    var query = HttpUtility.ParseQueryString(request.Url.Query);
                    var url = query.Get(0);
                    var fileName = query.Get(1);
                   
                    WebClient myWebClient = new WebClient();
                    var path = serverConfigs.PluginPath == null? AppDomain.CurrentDomain.BaseDirectory : serverConfigs.PluginPath;
                    string myStringWebResource = url;
                    var downloadInfo = string.Format("Downloading File \"{0}\" from \"{1}\" .......\n\n", fileName, myStringWebResource);
                    log.Info(downloadInfo);
                    Console.WriteLine(downloadInfo);
                    myWebClient.DownloadFile(myStringWebResource, path + fileName);
                    downloadInfo = string.Format("Successfully Downloaded File \"{0}\" from \"{1}\"", fileName, myStringWebResource);
                    log.Info(downloadInfo);
                    Console.WriteLine(downloadInfo);
                }).Start();

                return new ServiceActionResult() { Result = true, Message = "true", HttpResponseCode = 200 };
                //return "Download started";
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                log.Error(e.StackTrace);
                return new ServiceActionResult() { Result = true, Message = e.Message, HttpResponseCode = 500 };
            }

        }
    }
}
