using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterLib.CommandLineParser;
using TwitterLib.Load;

namespace TwitterLoader
{
    [Verb(Name = "Rebuild", Description = "Rebuilds indexes")]
    class Rebuild : DbVerbBase
    {
        [Parameter(Name = "DB", Description = "Target database.")]
        public string TwitterDb
        {
            get { return twitterDb; }
            set { twitterDb = value; }
        }

        public Rebuild()
            : base()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.twitterDb = "Twitter";
        }

        public override void Run()
        {
            var c = new Chunk();

            c.TargetDB.ConnectionString = GetConnectionString();
            c.TargetDB.InitialCatalog = twitterDb;
            c.RebuildIndexes();
        }
    }
}
