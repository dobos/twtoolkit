using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace TwitterLib.Methods
{
    public abstract class Filter : Sample
    {       
        public Filter()
            :base()
        {
        }

        protected override string GetHttpMethod()
        {
            return "POST";
        }

        protected override string GetPath()
        {
            return "https://stream.twitter.com/1.1/statuses/filter.json";
        }
    }
}
