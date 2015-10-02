using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TwitterLib;
using Spherical;
using Spherical.Htm;

namespace TwitterLib.Load.Mappers
{
    public class Tweet : Mapper
    {
        protected override string TableName
        {
            get { return "tweet"; }
        }

        public override void Map(Dictionary<string, object> obj)
        {
            if (obj.ContainsKey("text"))
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
            // [run_id] [smallint] NOT NULL
            BulkWriter.WriteSmallInt(RunID);

            // [tweet_id] [bigint] NOT NULL
            BulkWriter.WriteBigInt(JsonUtil.GetInt64(obj, "id"));

            // [created_at] [datetime] NOT NULL
            BulkWriter.WriteDateTime(JsonUtil.GetDateTime(obj, "created_at"));

            // [utc_offset] [int] NULL
            BulkWriter.WriteNullableInt(JsonUtil.GetNullableInt32(obj, "user.utc_offset"));

            // [user_id] [bigint] NOT NULL
            BulkWriter.WriteBigInt(JsonUtil.GetInt64(obj, "user.id"));

            // [place_id] [char](16) NULL
            BulkWriter.WriteNullableChar(JsonUtil.GetString(obj, "place.id"), 16);

            // [lon] [float] NULL
            // [lat] [float] NULL
            // [cx] [float] NOT NULL
	        // [cy] [float] NOT NULL
	        // [cz] [float] NOT NULL
            // [htm_id] [bigint] NOT NULL
            if (obj.ContainsKey("coordinates") && obj["coordinates"] != null)
            {
                string[] coords = (string[])((ArrayList)JsonUtil.GetValue(obj, "coordinates.coordinates")).ToArray(typeof(string));
                var lon = double.Parse(coords[0], System.Globalization.CultureInfo.InvariantCulture);
                var lat = double.Parse(coords[1], System.Globalization.CultureInfo.InvariantCulture);
                var c = new Cartesian(lon, lat);

                BulkWriter.WriteNullableFloat(lon);
                BulkWriter.WriteNullableFloat(lat);
                BulkWriter.WriteFloat(c.X);
                BulkWriter.WriteFloat(c.Y);
                BulkWriter.WriteFloat(c.Z);
                BulkWriter.WriteBigInt(Trixel.CartesianToHid20(c));
            }
            else
            {
                BulkWriter.WriteNullableFloat(null);
                BulkWriter.WriteNullableFloat(null);

                BulkWriter.WriteFloat(0);
                BulkWriter.WriteFloat(0);
                BulkWriter.WriteFloat(0);
                BulkWriter.WriteBigInt(0);
            }

            // [in_reply_to_tweet_id] [bigint] NULL
            BulkWriter.WriteNullableBigInt(JsonUtil.GetNullableInt64(obj, "in_reply_to_status_id"));

            // [in_reply_to_user_id] [bigint] NULL
            BulkWriter.WriteNullableBigInt(JsonUtil.GetNullableInt64(obj, "in_reply_to_user_id"));

            // [possibly_sensitive] [bit] NULL
	        // [possibly_sensitive_editable] [bit] NULL
            if (obj.ContainsKey("possibly_sensitive") && obj.ContainsKey("possibly_sensitive_editable"))
            {
                BulkWriter.WriteNullableBit(JsonUtil.GetNullableBoolean(obj, "possibly_sensitive"));
                BulkWriter.WriteNullableBit(JsonUtil.GetNullableBoolean(obj, "possibly_sensitive_editable"));
            }
            else
            {
                BulkWriter.WriteNullableBit(null);
                BulkWriter.WriteNullableBit(null);
            }

            // [retweet_count] [int] NOT NULL
            BulkWriter.WriteInt(JsonUtil.GetNullableInt32(obj, "retweet_count") ?? 0);

            // [text] [nvarchar](150) NOT NULL
            var text = Util.UnescapeText(System.Web.HttpUtility.HtmlDecode(JsonUtil.GetString(obj, "text")));
            BulkWriter.WriteVarChar(text, 150);

            // [truncated] [bit] NOT NULL
            BulkWriter.WriteBit(JsonUtil.GetNullableBoolean(obj, "truncated") ?? false);

            // [lang] [char](5) NOT NULL
            BulkWriter.WriteChar(JsonUtil.GetString(obj, "user.lang") ?? "??", 5);

            // [lang_word_count] [tinyint] NOT NULL
	        // [lang_guess1] [char](2) NOT NULL
	        // [lang_guess2] [char](2) NOT NULL
            int words;
            string lang1, lang2;
            if (LanguageUtil.DetectLanguage(text, out words, out lang1, out lang2))
            {
                BulkWriter.WriteTinyInt((sbyte)words);     // [lang_word_count]
                BulkWriter.WriteChar(lang1, 2);            // [lang_guess1]
                BulkWriter.WriteChar(lang2, 2);            // [lang_guess2]
            }
            else
            {
                BulkWriter.WriteTinyInt(0);
                BulkWriter.WriteChar("??", 2);
                BulkWriter.WriteChar("??", 2);
            }

            BulkWriter.EndLine();
        }

        
    }
}
