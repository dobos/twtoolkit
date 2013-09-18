using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitterLib;

namespace TwitterLib.Load.Mappers
{
    public class User : Mapper
    {
        protected override string TableName
        {
            get { return "user"; }
        }

        public override void Map(Dictionary<string, object> obj)
        {
            if (obj.ContainsKey("user"))
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
            var user = (Dictionary<string, object>)obj["user"];

            // [run_id] [smallint] NOT NULL
            BulkWriter.WriteSmallInt(RunID);

            // [user_id] [bigint] NOT NULL
            BulkWriter.WriteBigInt(JsonUtil.GetInt64(user, "id"));

            // [created_at] [datetime] NOT NULL
            BulkWriter.WriteDateTime(JsonUtil.GetDateTime(user, "created_at"));

            // [tweeted_at] [datetime] NOT NULL
            BulkWriter.WriteDateTime(JsonUtil.GetDateTime(obj, "created_at"));

            // [screen_name] [nvarchar](50) NOT NULL
            BulkWriter.WriteVarChar(JsonUtil.GetString(user, "screen_name") ?? "", 50);

            // [description] [nvarchar](160) NOT NULL
            BulkWriter.WriteVarChar(Util.UnescapeText(JsonUtil.GetString(user, "description")) ?? "", 160);
            
            // [favourites_count] [int] NOT NULL
            BulkWriter.WriteInt(JsonUtil.GetNullableInt32(user, "favourites_count") ?? 0);

            // [followers_count] [int] NOT NULL
            BulkWriter.WriteInt(JsonUtil.GetNullableInt32(user, "followers_count") ?? 0);

            // [friends_count] [int] NOT NULL
            BulkWriter.WriteInt(JsonUtil.GetNullableInt32(user, "friends_count") ?? 0);

            // [statuses_count] [int] NOT NULL
            BulkWriter.WriteInt(JsonUtil.GetNullableInt32(user, "statuses_count") ?? 0);

            // [geo_enabled] [bit] NOT NULL
            BulkWriter.WriteBit(JsonUtil.GetNullableBoolean(user, "geo_enabled") ?? false);

            // [lang] [char](5) NOT NULL,
            BulkWriter.WriteChar(JsonUtil.GetString(user, "lang") ?? "", 5);

            // [location] [nvarchar](100) NULL
            BulkWriter.WriteVarChar(Util.UnescapeText(JsonUtil.GetString(user, "location")), 100);

            // [name] [nvarchar](30) NOT NULL
            BulkWriter.WriteVarChar(Util.UnescapeText(JsonUtil.GetString(user, "name")) ?? "", 30);

            // [profile_background_color] [char](6) NOT NULL
            BulkWriter.WriteChar(JsonUtil.GetString(user, "profile_background_color") ?? "000000", 6);

            // [profile_text_color] [char](6) NOT NULL
            BulkWriter.WriteChar(JsonUtil.GetString(user, "profile_text_color") ?? "000000", 6);

            // [protected] [bit] NOT NULL
            BulkWriter.WriteBit(JsonUtil.GetNullableBoolean(user, "protected") ?? false);

            // [show_all_inline_media] [bit] NOT NULL
            BulkWriter.WriteBit(JsonUtil.GetNullableBoolean(user, "show_all_inline_media") ?? false);

            // [utc_offset] [int] NULL
            BulkWriter.WriteNullableInt(JsonUtil.GetNullableInt32(user, "utc_offset"));

            // [verified] [bit] NOT NULL
            BulkWriter.WriteBit(JsonUtil.GetNullableBoolean(user, "verified") ?? false);

            BulkWriter.EndLine();
        }
    }
}
