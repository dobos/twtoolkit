using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitterLib;

namespace TwitterLib.Load.Mappers
{
    public class TweetUserMention : Mapper
    {
        protected override string TableName
        {
            get { return "tweet_user_mention"; }
        }

        public override void Map(Dictionary<string, object> obj)
        {
            if (obj.ContainsKey("entities"))
            {
                MapOne(obj);

                if (obj.ContainsKey("retweeted_status"))
                {
                    MapOne((Dictionary<string, object>)obj["retweeted_status"]);
                }
            }
        }

        private void MapOne(Dictionary<string, object> obj)
        {
            ArrayList um = (ArrayList)JsonUtil.GetValue(obj, "entities.user_mentions");

            for (int i = 0; i < um.Count; i++)
            {
                if (!String.IsNullOrWhiteSpace((string)((Dictionary<string, object>)um[i])["id"]))
                {
                    // [run_id] [smallint] NOT NULL
                    BulkWriter.WriteSmallInt(RunID);

                    // [tweet_id] [bigint] NOT NULL
                    BulkWriter.WriteBigInt(JsonUtil.GetInt64(obj, "id"));

                    // [user_id] [bigint] NOT NULL
                    BulkWriter.WriteBigInt(JsonUtil.GetInt64(obj, "user.id"));

                    // [mentioned_user_id] [bigint] NOT NULL
                    BulkWriter.WriteBigInt(JsonUtil.GetInt64(um[i], "id"));

                    BulkWriter.EndLine();
                }
            }
        }

    }
}
