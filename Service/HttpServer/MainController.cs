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
using Services.Interfaces;

namespace HttpServer
{
    public class MainController
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public HttpServerConfigs configs { get; internal set; }

        private readonly IDownloadService downloadService;
        private readonly IMainInfoCollectorService mainInfoCollectorService;

        public MainController(HttpServerConfigs configs,
            IDownloadService downloadService,
            IMainInfoCollectorService mainInfoCollectorService)
        {
            this.configs = configs;
            this.downloadService = downloadService;
            this.mainInfoCollectorService = mainInfoCollectorService;
         
        }
        public ServiceActionResult InitDownload(HttpListenerRequest request)
        {
            var result = downloadService.DownloadAndInstallMSI(request);
            return result;
        }

        public ServiceActionResult DownloadDll(HttpListenerRequest request)
        {
            var result = downloadService.DownloadDll(request);
            return result;
        }


        public ServiceActionResult GetMainInfo(HttpListenerRequest request)
        {
            return mainInfoCollectorService.GetMainInfo(request);
        }

        public ServiceActionResult InvokeCustomModule(HttpListenerRequest request)
        {
            try
            {
                var query = HttpUtility.ParseQueryString(request.Url.Query);
                var fileName = query.GetParametr(0);
                var pluginClass = query.GetParametr(1);
                var method = query.GetParametr(2);
                var paramets = query.GetParametr(3);
                var path = configs.PluginPath ?? AppDomain.CurrentDomain.BaseDirectory;
                Assembly plugin = Assembly.LoadFile(path + fileName);
                //Assembly.ReflectionOnlyLoadFrom(@"C:\Users\KOMMANDE01\source\repos\TestLib\TestLib\bin\Debug\TestLib.dll");//
                // Get the type to use.
                Type pluginType = plugin.GetType(pluginClass);
                // Create an instance.
                object obj = Activator.CreateInstance(pluginType);
                // Get the method to call.
                MethodInfo pluginMethod = pluginType.GetMethod(method);
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
        public static string GetParametrWithRemove(this NameValueCollection col, int index)
        {
            var str =col.Get(index).Remove(0, 1);
            str = str.Remove(str.Length - 1, 1);
            return str;
        }
        public static string GetParametr(this NameValueCollection col, int index)
        {
            var str = col.Get(index);
            return str;
        }
    }
}
