using Models;
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

        public DownloadService(IInstallService installService)
        {
            this.installService = installService;
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
                    Console.WriteLine("Downloading File \"{0}\" from \"{1}\" .......\n\n", fileName, myStringWebResource);
                    myWebClient.DownloadFile(myStringWebResource, path + fileName);
                    Console.WriteLine("Successfully Downloaded File \"{0}\" from \"{1}\"", fileName, myStringWebResource);
                    Console.WriteLine("path:" + path);
                    Console.WriteLine("fileName: " + fileName);
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
    }
}
