using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterLib.Load.Mergers
{
    class TweetRetweet : Merger
    {
        protected override string SourceTableName
        {
            get { return "tweet_retweet"; }
        }

        protected override string TargetTableName
        {
            get { return "tweet_retweet"; }
        }
    }
}
