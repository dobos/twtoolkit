using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterLib.Load;
using TwitterLib.CommandLineParser;

namespace TwitterLoader
{
    [Verb(Name = "Prepare", Description = "Prepares chunk files")]
    class Prepare : LoadVerbBase
    {
        private bool skip;

        [Option(Name = "Skip", Description = "Skip preparing bulk load files if existing.")]
        public bool Skip
        {
            get { return skip; }
            set { skip = value; }
        }

        public override void Run()
        {
            Chunk[] chunks = GetChunks();

            // Schedule chunks for load
            TaskScheduler qts = GetScheduler();
            Task[] tasks = new Task[chunks.Length];

            for (int i = 0; i < chunks.Length; i++)
            {
                Task t = new Task(RunInternal, chunks[i]);
                t.Start(qts);

                tasks[i] = t;
            }

            Task.WaitAll(tasks);
        }

        private void RunInternal(object chunk)
        {
            ((Chunk)chunk).CreateFiles();
        }
    }
}
