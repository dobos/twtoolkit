using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterLib.Load;
using TwitterLib.CommandLineParser;

namespace TwitterLoader
{
    [Verb(Name = "Merge", Description = "Merge chunks with existing data")]
    class Merge : LoadVerbBase
    {
        private bool skipRebuild;

        [Option(Name = "SkipRebuild", Description = "No index rebuild.")]
        public bool SkipRebuild
        {
            get { return skipRebuild; }
            set { skipRebuild = value; }
        }

        [Parameter(Name = "DB", Description = "Target database.")]
        public string TwitterDb
        {
            get { return twitterDb; }
            set { twitterDb = value; }
        }

        public Merge()
            : base()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.twitterDb = "Twitter";
            this.skipRebuild = false;
        }

        public override void Run()
        {
            Chunk[] chunks = GetChunks();

            chunks[0].DisableIndexes();

            // This has to been done sequentially
            for (int i = 0; i < chunks.Length; i++)
            {
                chunks[i].TargetDB.InitialCatalog = twitterDb;
                chunks[i].MergeTables();
            }

            if (!skipRebuild)
            {
                chunks[0].RebuildIndexes();
            }
        }
    }
}
