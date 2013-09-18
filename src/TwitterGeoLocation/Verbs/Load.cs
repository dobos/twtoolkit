using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using TwitterLib.Load;
using TwitterLib.CommandLineParser;

namespace TwitterGeoLocation
{
    [Verb(Name = "Load", Description = "Load user locations.")]
    class Load : VerbBase
    {
        private string input;

        [Parameter(Name = "Input", Description = "Input file name.", Required = true)]
        public string Input
        {
            get { return input; }
            set { input = value; }
        }

        public Load()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.input = null;
        }

        public override void Run()
        {
            var sql = new StringBuilder(LoadScripts.bulkinsert);

            sql.Replace("$filename", input);
            sql.Replace("$dbname", TwitterDb);
            sql.Replace("$tablename", "user_friend");

            using (var cn = new SqlConnection(GetConnectionString()))
            {
                cn.Open();
                
                using (var cmd = new SqlCommand(sql.ToString(), cn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
