using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterLib.Load.Mergers
{
    class TweetUserMention : Merger
    {
        protected override string SourceTableName
        {
            get { return "tweet_user_mention"; }
        }

        protected override string TargetTableName
        {
            get { return "tweet_user_mention"; }
        }
    }
}
