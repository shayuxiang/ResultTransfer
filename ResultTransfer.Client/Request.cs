using System;
using System.Collections.Generic;
using System.Text;

namespace ResultTransfer.Client
{
    public class Request
    {
        public string Method { get; set; }
        public string URL { get; set; }
        public string ContentType { get; set; }
        public int ContentLength { get; set; }
        public string Host { get; set; }
        public string Data { get; set; }

        /// <summary>
        /// 构造一个request
        /// </summary>
        /// <param name="requestbody"></param>
        /// <returns></returns>
        public static Request Get(string requestbody)
        {
            var myrequests = requestbody.Split("\r\n");
            Request rq = new Request();
            //第一行
            var _stringchars = myrequests[0].Split(' ');
            rq.Method = _stringchars[0];
            rq.URL = _stringchars[1];
            foreach (var r in myrequests)
            {
                if (r.StartsWith("Content-Type", StringComparison.OrdinalIgnoreCase))
                {
                    var stringchars = r.Split(':');
                    rq.ContentType = stringchars[1].Trim();
                }
                if (r.StartsWith("Content-Length", StringComparison.OrdinalIgnoreCase))
                {
                    var stringchars = r.Split(':');
                    rq.ContentLength = int.Parse(stringchars[1].Trim());
                }
                if (r.StartsWith("Host", StringComparison.OrdinalIgnoreCase))
                {
                    var stringchars = r.Replace("Host:", string.Empty);
                    rq.Host = stringchars.Trim();
                }
            }
            //application/x-www-form-urlencoded
            if (rq.ContentType!=null && rq.ContentType.ToLower().Contains("application/x-www-form-urlencoded"))
            {
                rq.Data = myrequests[myrequests.Length - 1];
            }
            return rq;
        }
    }
}
