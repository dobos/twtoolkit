using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using TwitterLib;
using TwitterLib.Load;
using TwitterLib.CommandLineParser;

namespace TwitterLoader
{
    abstract class LoadVerbBase : DbVerbBase
    {
        protected short runId;
        private string input;
        private bool overlapped;
        private string bulkPath;
        private int threads;
        private bool binary;

        [Parameter(Name = "RunID", Description = "Run ID, must be inceremented by one each time.")]
        public short RunId
        {
            get { return runId; }
            set { runId = value; }
        }

        [Parameter(Name = "Input", Description = "Input file pattern.", Required = true)]
        public string Input
        {
            get { return input; }
            set { input = value; }
        }

        [Option(Name = "Overlapped", Description = "User overlapped IO for reading", Required = false)]
        public bool Overlapped
        {
            get { return overlapped; }
            set { overlapped = value; }
        }

        [Parameter(Name = "BulkPath", Description = "Temporary path for bulk loading.")]
        public string BulkPath
        {
            get { return bulkPath; }
            set { bulkPath = value; }
        }

        [Parameter(Name = "Treads", Description = "Multithreaded execution.")]
        public int Threads
        {
            get { return threads; }
            set { threads = value; }
        }

        [Option(Name = "Binary", Description = "Use binary files for bulk insert.")]
        public bool Binary
        {
            get { return binary; }
            set { binary = value; }
        }

        public LoadVerbBase()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.runId = 0;
            this.input = null;
            this.overlapped = false; ;
            this.bulkPath = null;
            this.threads = 1;
            this.binary = false;
        }

        protected TaskScheduler GetScheduler()
        {
            return new QueuedTaskScheduler(threads);
        }

        protected virtual Chunk[] GetChunks()
        {
            // Get files
            string dir = Path.GetDirectoryName(input);
            string pat = Path.GetFileName(input);
            string cstr = GetConnectionString();

            string[] files = Directory.GetFiles(dir, pat);

            Console.WriteLine("Found {0} chunks:", files.Length);

            // Configure chunks
            Chunk[] chunks = new Chunk[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                Chunk c = new Chunk()
                {
                    RunId = runId,
                    ID = i,
                    Binary = binary,
                    Filename = files[i],
                    ChunkId = Path.GetFileNameWithoutExtension(files[i]),
                    BulkPath = bulkPath,
                };

                chunks[i] = c;

                chunks[i].LoaderDB.ConnectionString = cstr;
                chunks[i].LoaderDB.InitialCatalog = loaderDb;

                Console.WriteLine(files[i]);
            }

            return chunks;
        }
    }
}
