using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Httpserver
{
    public class Response
    {
        private Byte[] data = null;
        private string status;
        private string mime;
        private Response(string Status , Byte[]data,string Mime)
        {
            this.data = data;
            this.status = Status;
            this.mime = Mime;

        }
        public static Response From(Request request)
        {
            if (request == null)
                return MakeNullResquest();
            if (request.Type .Equals( "GET"))
            {
                string file = Environment.CurrentDirectory + HttpServer.WebDir +request.Url;
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Exists&&fileInfo.Extension.Contains("."))
                {
                    return MakeFromFile(fileInfo);
                }
                else
                {

                    DirectoryInfo di =new DirectoryInfo(file+"/");
                    if (!di.Exists)
                    {
                        return MakePageNotFound();
                    }
                    FileInfo[] files = di.GetFiles();
                    foreach(FileInfo ff in files)
                    {
                        string filename=ff.Name;
                        if (filename.Contains("default.html") || filename.Contains("index.html") || filename.Contains("index.htm")){
                            fileInfo = ff;
                            return MakeFromFile(fileInfo);
                        }
                    }
                }
                if (!fileInfo.Exists)
                {
                    return MakePageNotFound();
                }
            }
            else
            {
                return MakeNotAllowed(); 
            }
                return MakePageNotFound();
            
        }

        private static Response MakeFromFile(FileInfo fileInfo)
        {
            
            FileStream fileStream = fileInfo.OpenRead();
            BinaryReader binaryReader = new BinaryReader(fileStream);
            Byte[] d = new Byte[fileStream.Length];
            binaryReader.Read(d, 0, d.Length);
            fileStream.Close();
            return new Response("200 Ok", d, "html/text");
        }

        private static Response MakeNullResquest()
        {
            string file = Environment.CurrentDirectory +HttpServer.Directory+"400.html";
            FileInfo fileInfo = new FileInfo(file);
            FileStream fileStream = fileInfo.OpenRead();
            BinaryReader binaryReader   = new BinaryReader(fileStream);
            Byte[] d=new Byte[fileStream.Length];
            binaryReader.Read(d,0,d.Length);
            fileStream.Close();
            return new Response("400 bad Request",d, "text/html");
        }
        private static Response MakeNotAllowed()
        {
            string file = Environment.CurrentDirectory + HttpServer.Directory + "405.html";
            FileInfo fileInfo = new FileInfo(file);
            FileStream fileStream = fileInfo.OpenRead();
            BinaryReader binaryReader = new BinaryReader(fileStream);
            Byte[] d = new Byte[fileStream.Length];
            binaryReader.Read(d, 0, d.Length);
            fileStream.Close();
            return new Response("405 Method not allowed", d, "text/html");
        }
        private static Response MakePageNotFound()
        {
            string file = Environment.CurrentDirectory + HttpServer.Directory + "404.html";
            FileInfo fileInfo = new FileInfo(file);
            FileStream fileStream = fileInfo.OpenRead();
            BinaryReader binaryReader = new BinaryReader(fileStream);
            Byte[] d = new Byte[fileStream.Length];
            binaryReader.Read(d, 0, d.Length);
            return new Response("404 page not found", d, "text/html");
        }

        public void Post(NetworkStream stream) {
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;
            writer.WriteLine(
                String.Format("{0} {1}\r\nServer: {2}\r\nContent-Type: {3}\r\nAccept-Ranges: bytes\r\nContent-Length: {4}\r\n",
                HttpServer.Version,
                status,  
                HttpServer.Name,
                mime,
                data.Length)
                );

            if (stream != null)
            {
                stream.Write(data, 0, data.Length);
            }

        }

    }
}
