using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace TwitterLib.Methods
{
    public class Sample : TwitterStreamingMethodBase
    {
        protected override Uri GetUrl()
        {
            var url = new Uri("https://stream.twitter.com/1/statuses/sample.json");
            AppendStreamingParameters(ref url);

            return url;
        }
    }
}
