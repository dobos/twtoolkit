using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterLib.Load.Mergers
{
    class TweetHashtag : Merger
    {
        protected override string SourceTableName
        {
            get { return "tweet_hashtag"; }
        }

        protected override string TargetTableName
        {
            get { return "tweet_hashtag"; }
        }
    }
}
