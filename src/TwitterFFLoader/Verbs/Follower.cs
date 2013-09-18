using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using TwitterLib.Load;
using TwitterLib.CommandLineParser;

namespace TwitterFFLoader.Verbs
{
    [Verb(Name = "Follower", Description = "Load follower graph.")]
    class Follower : VerbBase
    {
        public override void Run()
        {
            var sql = new StringBuilder(LoadScripts.bulkinsert);

            sql.Replace("$filename", Source);
            sql.Replace("$dbname", TargetDB);
            sql.Replace("$tablename", "user_follower");

            using (var cn = OpenConnection())
            {
                using (var cmd = new SqlCommand(sql.ToString(), cn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
