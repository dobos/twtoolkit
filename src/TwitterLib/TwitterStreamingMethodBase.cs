using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterLib
{
    public abstract class TwitterStreamingMethodBase : TwitterMethodBase
    {
        private int count;
        private bool delimited;
        private bool stallWarnings;

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        public bool Delimited
        {
            get { return delimited; }
            set { delimited = value; }
        }

        public bool StallWarnings
        {
            get { return stallWarnings; }
            set { stallWarnings = value; }
        }

        public TwitterStreamingMethodBase()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.count = -1;
            this.delimited = true;
            this.stallWarnings = true;
        }

        protected void AppendStreamingParameters(ref Uri url)
        {
            if (count > 0)
            {
                AppendUrlParameter(ref url, "count", count.ToString());
            }

            if (delimited)
            {
                AppendUrlParameter(ref url, "delimited", "length");
            }

            if (stallWarnings)
            {
                AppendUrlParameter(ref url, "stall_warnings", "true");
            }
        }
    }
}
