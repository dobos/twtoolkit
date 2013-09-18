using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using TwitterLib.Load;
using TwitterLib.CommandLineParser;

namespace TwitterLoader
{
    abstract class DbVerbBase : Verb
    {
        protected string server;
        protected bool integratedSecurity;
        protected string userId;
        protected string password;
        protected string loaderDb;
        protected string twitterDb;

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

        public DbVerbBase()
            : base()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.server = "localhost";
            this.integratedSecurity = true;
            this.userId = null;
            this.password = null;
            this.loaderDb = "TwitterLoader";
            this.twitterDb = "Twitter";
        }

        protected string GetConnectionString()
        {
            // Build connection string
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
