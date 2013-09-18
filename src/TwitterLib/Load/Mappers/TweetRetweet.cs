using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitterLib;

namespace TwitterLib.Load.Mappers
{
    public class TweetRetweet : Mapper
    {
        protected override string TableName
        {
            get { return "tweet_retweet"; }
        }

        public override void Map(Dictionary<string, object> obj)
        {
            if (obj.ContainsKey("text"))
            {
                if (obj.ContainsKey("retweeted_status"))
                {
                    MapOne(obj);
                }
            }
        }

        private void MapOne(Dictionary<string, object> obj)
        {
            // [run_id] [smallint] NOT NULL
            BulkWriter.WriteSmallInt(RunID);

            // [tweet_id] [bigint] NOT NULL
            BulkWriter.WriteBigInt(JsonUtil.GetInt64(obj, "id"));

            // [user_id] [bigint] NOT NULL
            BulkWriter.WriteBigInt(JsonUtil.GetInt64(obj, "user.id"));

            // [retweeted_tweet_id] [bigint] NOT NULL
            BulkWriter.WriteBigInt(JsonUtil.GetInt64(obj, "retweeted_status.id"));

            // [retweeted_user_id] [bigint] NOT NULL
            BulkWriter.WriteBigInt(JsonUtil.GetInt64(obj, "retweeted_status.user.id"));

            // [created_at] [datetime] NOT NULL
            BulkWriter.WriteDateTime(JsonUtil.GetDateTime(obj, "created_at"));

            // [retweeted_at] [datetime] NOT NULL
            BulkWriter.WriteDateTime(JsonUtil.GetDateTime(obj, "retweeted_status.created_at"));

            BulkWriter.EndLine();
        }

    }
}
