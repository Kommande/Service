using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IMainInfoCollectorService
    {
        ServiceActionResult GetMainInfo(HttpListenerRequest request);
    }
}
