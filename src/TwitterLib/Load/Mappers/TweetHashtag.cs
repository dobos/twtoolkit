using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitterLib;

namespace TwitterLib.Load.Mappers
{
    public class TweetHashtag : Mapper
    {
        protected override string TableName
        {
            get { return "tweet_hashtag"; }
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
            ArrayList um = (ArrayList)JsonUtil.GetValue(obj, "entities.hashtags");

            for (int i = 0; i < um.Count; i++)
            {
                // [run_id] [smallint] NOT NULL
                BulkWriter.WriteSmallInt(RunID);

                // [tweet_id] [bigint] NOT NULL
                BulkWriter.WriteBigInt(JsonUtil.GetInt64(obj, "id"));

                // [user_id] [bigint] NOT NULL
                BulkWriter.WriteBigInt(JsonUtil.GetInt64(obj, "user.id"));

                // [tag] [nvarchar](50) NOT NULL
                //string tag = (string)((Dictionary<string, object>)um[i])["text"];
                //WriteTinyInt(tag.Substring(0, Math.Min(tag.Length, 50)));
                BulkWriter.WriteVarChar(JsonUtil.GetString(um[i], "text"), 50);

                // [created_at] [datetime] NOT NULL
                BulkWriter.WriteDateTime(JsonUtil.GetDateTime(obj, "created_at"));

                BulkWriter.EndLine();
            }
        }

    }
}
