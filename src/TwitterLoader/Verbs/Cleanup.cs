using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterLib.Load;
using TwitterLib.CommandLineParser;

namespace TwitterLoader
{
    [Verb(Name = "Cleanup", Description = "Clean up after loading.")]
    class Cleanup : LoadVerbBase
    {
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

        private void RunInternal(object chunks)
        {
            ((Chunk)chunks).DropTables();
            ((Chunk)chunks).DeleteFiles();
        }
    }
}
