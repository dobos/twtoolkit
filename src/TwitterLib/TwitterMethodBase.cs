using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Configuration;
using System.Security.Cryptography;

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

        protected abstract string GetPath();

        protected virtual void GetQueryParameters(Dictionary<string, string> parameters)
        {
        }

        protected virtual void GetPostData(Dictionary<string, string> parameters)
        {
        }

        private string ConcatParameters(Dictionary<string, string> parameters)
        {
            var res = "";

            foreach (var key in parameters.Keys.OrderBy(i => i))
            {
                if (res != "")
                {
                    res += "&";
                }

                res += Uri.EscapeDataString(key) + "=" + Uri.EscapeDataString(parameters[key]);
            }

            return res;
        }

        private string GetQueryString()
        {
            var parameters = new Dictionary<string, string>();
            GetQueryParameters(parameters);
            return ConcatParameters(parameters);
        }

        private string GetPostDataString()
        {
            var parameters = new Dictionary<string, string>();
            GetPostData(parameters);
            return ConcatParameters(parameters);
        }

        public virtual HttpWebRequest GetHttpWebRequest()
        {
            var method = GetHttpMethod();
            var path = GetPath();
            var query = GetQueryString();
            var postdata = GetPostDataString();

            var url = path + (query == null ? "" : "?" + query);
            var auth = GetAuthorizationHeader(method, path, query, postdata);

            ServicePointManager.Expect100Continue = false;

            // Create and inizialize web request
            var req = (HttpWebRequest)HttpWebRequest.Create(url);

            req.Method = GetHttpMethod();
            req.Headers.Add("Authorization", auth);

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
            req.UserAgent = "ELTE Twitter Stream Reader";

            return req;
        }

        private string GetAuthorizationHeader(string method, string path, string query, string postData)
        {
            var oauthVersion = "1.0";
            var oauthSignatureMethod = "HMAC-SHA1";
            var oauthNonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var oauthTimestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            var url = path + query == null ? "" : "?" + query;
            var parameters = new Dictionary<string, string>();

            GetQueryParameters(parameters);
            GetPostData(parameters);

            parameters.Add("oauth_consumer_key", ConfigurationManager.AppSettings["TwitterConsumerKey"]);
            parameters.Add("oauth_nonce", oauthNonce);
            parameters.Add("oauth_signature_method", oauthSignatureMethod);
            parameters.Add("oauth_timestamp", oauthTimestamp);
            parameters.Add("oauth_token", ConfigurationManager.AppSettings["TwitterAccessToken"]);
            parameters.Add("oauth_version", oauthVersion);

            var baseString = ConcatParameters(parameters);

            baseString = String.Format(
                "{0}&{1}&{2}",
                method,
                Uri.EscapeDataString(path),
                Uri.EscapeDataString(baseString));

            var compositeKey = String.Format(
                "{0}&{1}",
                Uri.EscapeDataString(ConfigurationManager.AppSettings["TwitterConsumerSecret"]),
                Uri.EscapeDataString(ConfigurationManager.AppSettings["TwitterAccessTokenSecret"]));

            string oauthSignature;
            using (var hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            {
                oauthSignature = Convert.ToBase64String(hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            var headerFormat =
                "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                "oauth_version=\"{6}\"";

            var authHeader = string.Format(
                headerFormat,
                Uri.EscapeDataString(oauthNonce),
                Uri.EscapeDataString(oauthSignatureMethod),
                Uri.EscapeDataString(oauthTimestamp),
                Uri.EscapeDataString(ConfigurationManager.AppSettings["TwitterConsumerKey"]),
                Uri.EscapeDataString(ConfigurationManager.AppSettings["TwitterAccessToken"]),
                Uri.EscapeDataString(oauthSignature),
                Uri.EscapeDataString(oauthVersion));

            return authHeader;
        }
    }
}
