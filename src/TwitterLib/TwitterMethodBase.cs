using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Configuration;

namespace TwitterLib
{
    public abstract class TwitterMethodBase
    {
        public TwitterMethodBase()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
        }

        protected virtual string GetHttpMethod()
        {
            return "GET";
        }

        protected abstract Uri GetUrl();

        protected virtual string GetPostData()
        {
            return null;
        }

        protected void AppendUrlParameter(ref Uri url, string name, string value)
        {
            var ub = new UriBuilder(url);

            if (!String.IsNullOrEmpty(ub.Query))
            {
                ub.Query += "&";
            }

            ub.Query += String.Format("{0}={1}", name, value);

            url = ub.Uri;
        }

        public virtual HttpWebRequest GetHttpWebRequest()
        {
            // Create and inizialize web request
            var req = (HttpWebRequest)HttpWebRequest.Create(GetUrl());

            req.Method = GetHttpMethod();

            var postdata = GetPostData();
            
            if (postdata != null)
            {
                var encoding = new ASCIIEncoding();
                var data = encoding.GetBytes(postdata);
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = data.Length;
                var reqstream = req.GetRequestStream();
                
                // Send the data.
                reqstream.Write(data, 0, data.Length);
                reqstream.Close();
            }


            // Set common parameters
            req.AllowAutoRedirect = false;
            req.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
            req.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["TwitterLib.Username"], ConfigurationManager.AppSettings["TwitterLib.Password"]);

            // Mono fails with KeepAlive=true
            req.KeepAlive = false;
            
            req.Pipelined = false;
            req.PreAuthenticate = false;
            req.UserAgent = "ELTE Complex Twitter Streaming Client";

            return req;
        }
    }
}
