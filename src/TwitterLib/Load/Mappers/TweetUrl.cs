using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitterLib;

namespace TwitterLib.Load.Mappers
{
    public class TweetUrl : Mapper
    {
        protected override string TableName
        {
            get { return "tweet_url"; }
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
            // Write URLs
            ArrayList um = (ArrayList)JsonUtil.GetValue(obj, "entities.urls");
            WriteUrl(obj, um);

            // Write media urls
            um = (ArrayList)JsonUtil.GetValue(obj, "entities.media");
            WriteUrl(obj, um);
        }

        private void WriteUrl(Dictionary<string, object> obj, ArrayList um)
        {
            if (um != null)
            {
                for (int i = 0; i < um.Count; i++)
                {
                    string url = (string)((Dictionary<string, object>)um[i])["url"];
                    if (url.StartsWith("http://t.co/"))
                    {
                        url = url.Substring(12);
                    }
                    else if (url.StartsWith("http://t.co/"))
                    {
                        url = url.Substring(13);
                    }
                    else
                    {
                        continue;
                    }

                    // [run_id] [smallint] NOT NULL
                    BulkWriter.WriteSmallInt(RunID);

                    // [tweet_id] [bigint] NOT NULL
                    BulkWriter.WriteBigInt(JsonUtil.GetInt64(obj, "id"));

                    // [user_id] [bigint] NOT NULL
                    BulkWriter.WriteBigInt(JsonUtil.GetInt64(obj, "user.id"));

                    // [url_id] [char](8) NOT NULL
                    BulkWriter.WriteChar(url, 8);

                    // [created_at] [datetime] NOT NULL
                    BulkWriter.WriteDateTime(JsonUtil.GetDateTime(obj, "created_at"));

                    // [expanded_url] [varchar](8000)
                    if (((Dictionary<string, object>)um[i]).ContainsKey("expanded_url"))
                    {
                        BulkWriter.WriteVarChar((string)((Dictionary<string, object>)um[i])["expanded_url"], 8000);
                    }
                    else
                    {
                        BulkWriter.WriteVarChar(null, 8000);
                    }

                    BulkWriter.EndLine();
                }
            }
        }
    }
}
