using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using TwitterLib.Load;
using TwitterLib.CommandLineParser;

namespace TwitterFFLoader.Verbs
{
    [Verb(Name = "Friend", Description = "Load friend graph.")]
    class Friend : VerbBase
    {
        public override void Run()
        {
            var sql = new StringBuilder(LoadScripts.bulkinsert);

            sql.Replace("$filename", Source);
            sql.Replace("$dbname", TargetDB);
            sql.Replace("$tablename", "user_friend");

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
