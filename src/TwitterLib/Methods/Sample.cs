using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace TwitterLib.Methods
{
    public class Sample : TwitterStreamingMethodBase
    {
        protected override string GetPath()
        {
            return "https://stream.twitter.com/1.1/statuses/sample.json";
        }
    }
}
