using Models;
using Models.Configs;
using Services;
using Services.Impl;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HttpServer
{
    public class Server
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string _mainUrl;
        private HttpListener _httpListener;
        private readonly IDownloadService downloadService;
        private readonly IInstallService installService;
        private readonly IMainInfoCollectorService mainInfoCollectorService;
        public HttpServerConfigs configs { get; internal set; }
        public Server(string mainUrl, HttpServerConfigs configs)
        {
            _mainUrl = mainUrl;
            this.configs = configs;
            this.installService = new InstallService();
            this.downloadService = new DownloadService(installService,configs);
            this.mainInfoCollectorService = new MainInfoCollectorService();

        }

        private void Init()
        {
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(_mainUrl);
            
            log4net.Config.XmlConfigurator.Configure();
        }

        public void StartServer()
        {
            Init();
            _httpListener.Start();
            Console.WriteLine("Ожидание подключений...");
            log.Info("Ожидание подключений...");
            while (true)
            {
                try
                {
                    var context = _httpListener.GetContext();
                    ThreadPool.QueueUserWorkItem(c => HandleRequest(context));
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                    log.Error(e.StackTrace);
                   // throw;
                } 
            }

            
        }

       

        private void HandleRequest(HttpListenerContext context)
        {
            try
            {
               // var currentContext = (HttpListenerContext)state;
               
                var controller =  new MainController(configs,downloadService,mainInfoCollectorService);
                if (configs.ServerUrl!= null && context.Request.RemoteEndPoint.Address.ToString() != configs.ServerUrl)
                {
                    throw new Exception("Invalid Remote Server");
                }
                var requestedMethod = context.Request.Url.AbsolutePath.Remove(0,1);
                //requestedMethod = requestedMethod.Remove(requestedMethod.Length - 1, 1);
                Console.WriteLine("target method name: "+requestedMethod);
                log.Info("target method name: " + requestedMethod);
                var method = controller.GetType().GetMethod(requestedMethod);
                var result = (ServiceActionResult)method.Invoke(controller, new object[] { context.Request });

                context.Response.StatusCode = result.HttpResponseCode;
                context.Response.SendChunked = true;
                context.Response.ContentType = "application/json";
                var bytes = Encoding.UTF8.GetBytes((string) result.Message);
                context.Response.OutputStream.Write(bytes,0,bytes.Length);
                context.Response.Close();
                //int totalTime = 0;                
            }

            catch (Exception e)
            {
                log.Error(e.Message);
                log.Error(e.StackTrace);
                context.Response.StatusCode = 500;
                context.Response.SendChunked = true;
                var bytes = Encoding.UTF8.GetBytes(e.Message);
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                context.Response.Close();
            }
        }
    }
}
