using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ResultTransfer.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var ret = MQBuilder.Instance.CustomerReciver("MyResultTransfer", "TransferRoute");
                Request rq = Request.Get(ret); //构造一个rq
                ProxyRequest proxyRequest = new ProxyRequest(); //代理请求
                var response = proxyRequest.Send(rq);
                Console.WriteLine(response);
            }
        }
    }
}
