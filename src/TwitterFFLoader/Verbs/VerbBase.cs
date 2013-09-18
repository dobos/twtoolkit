using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using TwitterLib.CommandLineParser;

namespace TwitterFFLoader.Verbs
{
    abstract class VerbBase : Verb
    {
        private short runID;
        private string source;
        private string targetDB;
        //private string loaderDB;
        private string server;
        private bool integratedSecurity;
        private string userID;
        private string password;
        private bool binary;

        [Parameter(Name = "RunID", Description = "RUN_ID", Required = true)]
        public short RunID
        {
            get { return runID; }
            set { runID = value; }
        }

        [Parameter(Name = "Source", Description = "Source file pattern.", Required = true)]
        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        [Parameter(Name = "TargetDB", Description = "Database to use as target.")]
        public string TargetDB
        {
            get { return targetDB; }
            set { targetDB = value; }
        }

        /*[Parameter(Name = "LoaderDB", Description = "Database to use for loading.")]
        public string LoaderDB
        {
            get { return loaderDB; }
            set { loaderDB = value; }
        }*/

        [Parameter(Name = "Server", Description = "Database server with target and load DB co-located.")]
        public string Server
        {
            get { return server; }
            set { server = value; }
        }

        [Option(Name = "EnableIntegratedSecurity", Description = "Use Windows login.")]
        public bool IntegratedSecurity
        {
            get { return integratedSecurity; }
            set { integratedSecurity = value; }
        }

        [Parameter(Name = "UserId", Description = "User ID.")]
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        [Parameter(Name = "Password", Description = "Password")]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        [Option(Name = "Binary", Description = "User binary files for bulk insert.")]
        public bool Binary
        {
            get { return binary; }
            set { binary = value; }
        }

        public VerbBase()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.runID = 0;
            this.source = null;
            this.targetDB = Constants.DefaultTargetDB;
            //this.loaderDB = Constants.DefaultLoaderDB;
            this.server = Constants.DefaultServer;
            this.integratedSecurity = true;
            this.userID = String.Empty;
            this.password = String.Empty;
            this.binary = false;
        }

        protected SqlConnection OpenConnection()
        {
            var csb = new SqlConnectionStringBuilder();

            csb.DataSource = server;

            if (integratedSecurity)
            {
                csb.IntegratedSecurity = true;
            }
            else
            {
                csb.IntegratedSecurity = false;
                csb.UserID = userID;
                csb.Password = password;
            }

            var cn = new SqlConnection(csb.ConnectionString);
            cn.Open();

            return cn;
        }
    }
}
