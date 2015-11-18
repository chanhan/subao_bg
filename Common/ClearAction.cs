using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Configuration;

namespace Common
{
    public static class ClearAction
    {
        public static void Clear(string action)
        {
            string clearWeb = WebConfigurationManager.AppSettings["clearWeb"];
            string[] ips = clearWeb.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            string[] urls = (from ip in ips select String.Format("http://{0}/ajax/CacheAction.aspx?ActionName={1}", ip.Trim(), action)).Distinct().ToArray();

            SendRequest(urls);
        }

        private static void SendRequest(string[] urls)
        {
            if (urls == null || urls.Length == 0) { return; }

            foreach (string url in urls)
            {
                try
                {
                    HttpWebRequest req = HttpWebRequest.Create(url) as HttpWebRequest;
                    req.Method = "GET";
                    req.BeginGetResponse(null, null);

                    Thread.Sleep(500);
                }
                catch
                {
                }
            }
        }
    }
}
