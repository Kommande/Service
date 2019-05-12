using Hardware;
using Models;
using Newtonsoft.Json;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Services.Impl
{
    public class MainInfoCollectorService:IMainInfoCollectorService
    {
        public ServiceActionResult GetMainInfo(HttpListenerRequest request)
        {
            var query = HttpUtility.ParseQueryString(request.Url.Query, Encoding.UTF8);
            var programmString = query.Get(0);//network.AcceptCommand();
            programmString = HttpUtility.UrlDecode(programmString);
            programmString = programmString.Remove(0, 1);
            programmString = programmString.Remove(programmString.Length - 1, 1);
           // var commandParser = new CommandParser();
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
    }
}
