using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public class CommandParser
    {
        public List<string> ParsePath(string pathString)
        {
            try
            {
                Console.WriteLine("Parse try path");
                var obj = JArray.Parse(pathString.ToString().Replace('"', '\''));
                return obj.ToList().Select(x => x.Value<string>("path")).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("Parse failed");
            }
            return new List<string>();
        }
    }
}
