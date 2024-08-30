using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Httpserver
{
    public class Request
    {
        public String Url { get; set; }
        public String Type { get; set; }
        public String Host { get; set; }

        private Request(string url, string type, string Host)
        {
            Url = url;
            Type = type;
            this.Host = Host;
        }
        public static Request GetRequest(String Request)
        {
            if (String.IsNullOrEmpty(Request))
                return null;
            string []Tokens= Request.Split(" ");
            if (Tokens.Length == 1)
            {
                return null;
            }
            string type=Tokens[0];
            string url= Tokens[1];
            string host=Tokens[4];  
            return new Request(url, type, host);
        }
    }
}
