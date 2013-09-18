using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterLib.Load.Mergers
{
    class UserHashtag : Merger
    {
        protected override string SourceTableName
        {
            get { return "tweet_hashtag"; }
        }

        protected override string TargetTableName
        {
            get { return "user_hashtag"; }
        }
    }
}
