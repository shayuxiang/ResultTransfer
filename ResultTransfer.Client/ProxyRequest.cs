using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ResultTransfer.Client
{
    public class ProxyRequest
    {
        public string ProxyHost { get; set; } = "http://localhost:15251";

        private Request Request { get; set; }

        public Task<string> Send(Request rq) {
            this.Request = rq;
            if (rq.Method == "GET")
            {
                return GetContent(rq.URL, Encoding.UTF8);
            }
            else 
            {
                return PostContent(rq.URL,rq.Data, Encoding.UTF8);
            }
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="coding"></param>
        /// <returns></returns>
        public async Task<string> GetContent(string uri, Encoding coding)
        {
            try
            {
                //Get请求中请求参数等直接拼接在url中
                WebRequest request = WebRequest.Create($@"{ProxyHost}{uri}");

                //返回对Internet请求的响应
                WebResponse resp = await request.GetResponseAsync();

                //从网络资源中返回数据流
                Stream stream = resp.GetResponseStream();

                StreamReader sr = new StreamReader(stream, coding);

                //将数据流转换文字符串
                string result = sr.ReadToEnd();

                //关闭流数据
                stream.Close();
                sr.Close();

                return result;
            } catch {
                return "服务器错误";
            }
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="data"></param>
        /// <param name="coding"></param>
        /// <returns></returns>
        public async Task<string> PostContent(string uri, string data, Encoding coding)
        {
            WebRequest request = WebRequest.Create($@"{ProxyHost}{uri}");
            request.ContentType = Request.ContentType;
            request.Method = Request.Method;
            request.ContentLength = Request.ContentLength;

            byte[] buffer = null;
            Stream stream = null;
            if (!string.IsNullOrEmpty(data))
            {
                //将字符串数据转化为字节串,这也是POST请求与GET请求区别的地方
                buffer = coding.GetBytes(data);

                //用于将数据写入Internet资源
                stream = request.GetRequestStream();
                stream.Write(buffer, 0, buffer.Length);
            }
            Console.WriteLine($@"正在请求{ProxyHost}{uri}===>{data}");
            WebResponse response = await request.GetResponseAsync();

            try
            {
                //从网络资源中返回数据流
                stream = response.GetResponseStream();

                StreamReader sr = new StreamReader(stream, coding);

                //将数据流转换文字符串
                string result = sr.ReadToEnd();

                //关闭流数据
                stream.Close();
                sr.Close();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"请求错误{ProxyHost}{uri}===>{ex.Message}");
                return string.Empty;
            }
        }
    }
}
