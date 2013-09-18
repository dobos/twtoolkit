using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitterLib;

namespace TwitterLib.Load.Mappers
{
    public class ScrubGeo : Mapper
    {
        protected override string TableName
        {
            get { return "scrub_geo"; }
        }

        public override void Map(Dictionary<string, object> obj)
        {
            if (obj.ContainsKey("scrub_geo"))
            {
                MapOne(obj);
            }
        }

        private void MapOne(Dictionary<string, object> obj)
        {
            // [run_id] [smallint] NOT NULL
            BulkWriter.WriteSmallInt(RunID);

            // [user_id] [bigint] NOT NULL
            BulkWriter.WriteBigInt(JsonUtil.GetInt64(obj, "scrub_geo.user_id"));

            // [up_to_status_id] [bigint] NOT NULL
            BulkWriter.WriteBigInt(JsonUtil.GetInt64(obj, "scrub_geo.up_to_status_id"));

            BulkWriter.EndLine();
        }
    }
}
