using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using TwitterLib.CommandLineParser;

namespace TwitterGeoLocation
{
    abstract class VerbBase : Verb
    {
        private short runId;
        private string server;
        private bool integratedSecurity;
        private string userId;
        private string password;
        private string loaderDb;
        private string twitterDb;

        [Parameter(Name = "RunID", Description = "Run ID, must be inceremented by one each time.")]
        public short RunId
        {
            get { return runId; }
            set { runId = value; }
        }

        [Parameter(Name = "Server", Description = "Database server.")]
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
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        [Parameter(Name = "Password", Description = "Password")]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        [Parameter(Name = "LoaderDB", Description = "Database to use for loading.")]
        public string LoaderDb
        {
            get { return loaderDb; }
            set { loaderDb = value; }
        }

        [Parameter(Name = "TwitterDb", Description = "Database to get data from/store data to.")]
        public string TwitterDb
        {
            get { return twitterDb; }
            set { twitterDb = value; }
        }

        public VerbBase()
            : base()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.runId = -1;
            this.server = "localhost";
            this.integratedSecurity = true;
            this.userId = null;
            this.password = null;
            this.loaderDb = "TwitterLoader";
            this.twitterDb = "Twitter";
        }

        /// <summary>
        /// Build connection string
        /// </summary>
        /// <returns></returns>
        protected string GetConnectionString()
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = server;
            csb.InitialCatalog = twitterDb;
            if (integratedSecurity)
            {
                csb.IntegratedSecurity = true;
            }
            else
            {
                csb.IntegratedSecurity = false;
                csb.UserID = userId;
                csb.Password = password;
            }

            return csb.ConnectionString;
        }
    }
}
