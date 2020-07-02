using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ResultTransfer.Server
{
    public class SimpleWebServer
    {
        private TcpListener myListener;
        private int port = 1111; // 选者任何闲置端口

        //开始兼听端口
        //同时启动一个兼听进程
        public void Run()
        {
            try
            {
                //开始兼听端口
                myListener = new TcpListener(IPAddress.Any, port);
                myListener.Start();
                Console.WriteLine("Web Server Running... Press ^C to Stop...");
                Thread th = new Thread(new ThreadStart(ListeningRequest));
                th.Start();

            }
            catch (Exception e)
            {
                Console.WriteLine("兼听端口时发生错误 :" + e.ToString());
            }
        }

        public void ListeningRequest() {
            while (true)
            {
                //接受新连接
                Socket mySocket = myListener.AcceptSocket();
                Console.WriteLine("Socket Type " + mySocket.SocketType);
                if (mySocket.Connected)
                {
                    Console.WriteLine("\nClient Connected!!\n==================\nCLient IP {0}\n", mySocket.RemoteEndPoint);
                    Byte[] bReceive = new Byte[4096];
                    int i = mySocket.Receive(bReceive, bReceive.Length, 0);

                    //转换成字符串类型
                    string sBuffer = Encoding.ASCII.GetString(bReceive);
                    MQBuilder.Instance.ProducterSender("MyResultTransfer", "TransferRoute", sBuffer);
                }
            }
        }

    }
}
