using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading;
using HttpServer;
using Models.IpFilter;

namespace Network
{
    public class Network
    {
        public static int port = 6666;
        public Socket ServerSocket { get; private set; }
        public Socket ListenSocket { get; private set; }

        public IpV4Filter filter { get; private set; }

        public Network(IpV4Filter filter)
        {
            this.filter = filter;
        }

        public void BindSocket()
        {

        }
        public string AcceptCommand()
        {
            IPHostEntry iPHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress iPAddress =  IPAddress.Parse("127.0.0.1");
            foreach (var v in iPHost.AddressList)
            {
                Console.WriteLine(": " + v);
            }
            foreach (var v in iPHost.AddressList)
            {
                Console.WriteLine(": "+v);
                if (v.AddressFamily.Equals(AddressFamily.InterNetwork))
                {
                    if (CheckAddres(v))
                    {
                        iPAddress = v;
                        break;
                    }
                }
            }
            Console.WriteLine("Жду подключения по адресу: {0}", iPAddress);
            //IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, port);
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, port);
            Socket listenSocket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            StringBuilder data = new StringBuilder();
           
            try
            {
                listenSocket.Bind(iPEndPoint);
                listenSocket.Listen(1);
                byte[] buffer;
                while (true)
                {
                     ServerSocket = listenSocket.Accept();
                     Console.WriteLine("Подключился");
                     Console.WriteLine("ServerSocket.Connected:{0}",ServerSocket.Connected);
                    while (true)
                    {
                        buffer = new byte[1024];
                        Console.WriteLine("before receive");
                        int butesRec = ServerSocket.Receive(buffer);
                        Console.WriteLine("after receive");
                        data.Append(Encoding.ASCII.GetString(buffer, 0, butesRec));
                        Console.WriteLine(data.ToString());
                        if (ServerSocket.Available == 0)
                        {
                            Console.WriteLine("End of receiving");
                            break;
                        }
                    }
                    Console.WriteLine("received: "+ data.ToString());
                    try
                    {
                        Console.WriteLine("Parse try all");
                        var obj = JArray.Parse(data.ToString().Replace('"', '\''));
                        obj.ToList().ForEach(x => Console.WriteLine(x.Value<string>("path")));
                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.StackTrace);
                        Console.WriteLine("continue");
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            listenSocket.Dispose();
            return data.ToString();
        } 
        public void SendResult(string result)
        {
            byte[] res = Encoding.UTF8.GetBytes(result);
            try
            {
                if (ServerSocket.Connected)
                {
                    NetworkStream networkStream = new NetworkStream(ServerSocket);
                    if (!networkStream.CanWrite)
                    {
                        Console.WriteLine("Не могу писать в поток");
                        return;
                    }
                    /*StreamWriter writer = new StreamWriter(networkStream, Encoding.UTF8);
                    Console.WriteLine("Пробую записать в поток");
                    writer.Write(result);
                    writer.Flush();
                    Console.WriteLine("Закрываю поток");
                    writer.Close();
                    networkStream.Close();*/
                    //networkStream.Write(res, 0, res.Length);
                    ServerSocket.Send(res);
                    ServerSocket.Shutdown(SocketShutdown.Both);
                    ServerSocket.Disconnect(true);
                    ServerSocket.Close();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
        public bool CheckAddres(IPAddress adress) 
        {
           
            var splitedAdress = adress.ToString().Split('.');
            if (this.filter.Ip1 != default(string))
            {
                if(splitedAdress[0].Equals(this.filter.Ip1))
                {
                    if (this.filter.Ip2 != default(string))
                    {
                        if (splitedAdress[1].Equals(this.filter.Ip2))
                        {
                            if (this.filter.Ip3 != default(string))
                            {
                                if (splitedAdress[2].Equals(this.filter.Ip3))
                                {
                                    if (this.filter.Ip4 != default(string))
                                    {
                                        if (splitedAdress[3].Equals(this.filter.Ip4))
                                        {
                                            return true;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return true;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
            return false;
        }

        private string GetMainUrl()
        {
            IPHostEntry iPHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
           /* foreach (var v in iPHost.AddressList)
            {
                Console.WriteLine(": " + v);
            }*/
            foreach (var v in iPHost.AddressList)
            {
                //Console.WriteLine(": " + v);
                if (v.AddressFamily.Equals(AddressFamily.InterNetwork))
                {
                    if (CheckAddres(v))
                    {
                        iPAddress = v;
                        break;
                    }
                }
            }
           
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, port);
            var address = $"http://{iPEndPoint.Address.ToString()}:6666/";
            Console.WriteLine(address);
            return address;
        }

        public void StartHttpServer()
        {
            var mainUrl = GetMainUrl();
            var server = new Server(mainUrl);
            server.StartServer();
        }
    }
}
