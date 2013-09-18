using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterLib;
using TwitterLib.Load;
using TwitterLib.CommandLineParser;

namespace TwitterLoader
{
    [Verb(Name = "Batch", Description = "Batch loading of many chunks.")]
    class Batch : LoadVerbBase
    {
        private int batchSize;
        private bool skipPrepare;
        private bool skipRebuild;

        [Parameter(Name = "DB", Description = "Target database.")]
        public string TwitterDb
        {
            get { return twitterDb; }
            set { twitterDb = value; }
        }

        [Parameter(Name = "BatchSize", Description = "Batch size.", Required = true)]
        public int BatchSize
        {
            get { return batchSize; }
            set { batchSize = value; }
        }

        [Option(Name = "SkipPrepare", Description = "Skip preparing bulk load files if existing")]
        public bool SkipPrepare
        {
            get { return skipPrepare; }
            set { skipPrepare = value; }
        }

        [Option(Name = "NoRebuild", Description = "No index rebuild.")]
        public bool SkipRebuild
        {
            get { return skipRebuild; }
            set { skipRebuild = value; }
        }

        public Batch()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.batchSize = 10;
        }

        public override void Run()
        {
            Chunk[] chunks = GetChunks();
            int cstart = 0;

            chunks[0].DisableIndexes();

            while (cstart < chunks.Length)
            {
                int ccount = Math.Min(batchSize, chunks.Length - cstart);

                // Schedule chunks for load
                TaskScheduler qts = GetScheduler();
                Task[] tasks = new Task[ccount];

                // Prepare and Load
                for (int i = 0; i < ccount; i++)
                {
                    tasks[i] = new Task(PrepareAndLoad, chunks[cstart + i]);
                    tasks[i].Start(qts);
                }

                Task.WaitAll(tasks);

                // Merge
                // This has to been done sequentially
                for (int i = 0; i < ccount; i++)
                {
                    chunks[cstart + i].TargetDB.InitialCatalog = twitterDb;
                    chunks[cstart + i].MergeTables();
                }

                // Cleanup
                for (int i = 0; i < ccount; i++)
                {
                    tasks[i] = new Task(Cleanup, chunks[cstart + i]);
                    tasks[i].Start(qts);
                }

                Task.WaitAll(tasks);

                cstart += batchSize;
            }

            if (!skipRebuild)
            {
                chunks[0].RebuildIndexes();
            }
        }

        private void PrepareAndLoad(object chunk)
        {
            ((Chunk)chunk).CreateFiles();
            ((Chunk)chunk).CreateTables();
            ((Chunk)chunk).RunBulkInsert();
        }

        private void Cleanup(object chunks)
        {
            ((Chunk)chunks).DropTables();
            
            if (!skipPrepare)
            {
                ((Chunk)chunks).DeleteFiles();
            }
        }
    }
}
