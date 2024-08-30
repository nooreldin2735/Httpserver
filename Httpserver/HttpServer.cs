using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Httpserver
{
   public class HttpServer
    {
        public const string Directory= "/msg/";
        public const string WebDir = "/web/"; 
        public const string Version = "HTTP/1.1";
        public const string Name = "MyHttp server";
        private bool running;
        private TcpListener listener;
        public HttpServer(int port)
        {
            listener = new TcpListener(IPAddress.Any,port);
            
        }
        public void Start()
        {
            Thread thread = new  Thread(new ThreadStart(Run));
            thread.Start();

        }

        private void Run()
        {
            running = true;
            listener.Start();
            while (running) {
                Console.WriteLine("Waiting For Connection");
                TcpClient  client =listener.AcceptTcpClient();
                Console.WriteLine("Client Connected");
                HandelClient(client);
                client.Close();
            }
            running = false;
            listener.Stop();
        }

        private void HandelClient(TcpClient client)
        {
            StreamReader reader = new StreamReader(client.GetStream());
            string msg = "";
            while (reader.Peek() != -1)
            {
                msg += reader.ReadLine()+"\n";
            }
            Console.WriteLine("Reguest::" +msg );
            Request reg = Request.GetRequest(msg);
            Response response=Response.From(reg);
            response.Post(client.GetStream());

        }
    }
}
