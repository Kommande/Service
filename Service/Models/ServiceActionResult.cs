using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ServiceActionResult
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public int HttpResponseCode { get; set; }
    }
}
