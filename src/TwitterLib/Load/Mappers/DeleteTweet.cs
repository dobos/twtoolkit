using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitterLib;

namespace TwitterLib.Load.Mappers
{
    public class DeleteTweet : Mapper
    {
        protected override string TableName
        {
            get { return "delete_tweet"; }
        }

        public override void Map(Dictionary<string, object> obj)
        {
            if (obj.ContainsKey("delete") && ((Dictionary<string, object>)obj["delete"]).ContainsKey("status"))
            {
                MapOne(obj);
            }
        }

        private void MapOne(Dictionary<string, object> obj)
        {
            // [run_id] [smallint] NOT NULL
            BulkWriter.WriteSmallInt(RunID);

            // [tweet_id] [bigint] NOT NULL
            BulkWriter.WriteBigInt(JsonUtil.GetInt64(obj, "delete.status.id"));

            // [user_id] [bigint] NOT NULL
            BulkWriter.WriteBigInt(JsonUtil.GetInt64(obj, "delete.status.user_id"));

            BulkWriter.EndLine();
        }
    }
}
