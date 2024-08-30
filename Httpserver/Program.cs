namespace Httpserver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server Starting At port 9999...........");
            HttpServer http = new HttpServer(9999);
            http.Start();
        }
    }
}
