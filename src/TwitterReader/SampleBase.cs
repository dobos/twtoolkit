using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using TwitterLib;
using TwitterLib.CommandLineParser;

namespace TwitterReader
{
    abstract class SampleBase : Verb
    {
        private bool compress;
        private string output;
        private int fileLineMax;
        private int fileTimeMax;

        [Option(Name = "Compress", Description = "Compress output stream.", Required = false)]
        public bool Compress
        {
            get { return compress; }
            set { compress = value; }
        }

        [Parameter(Name = "Output", Description = "Output path.")]
        public string Output
        {
            get { return output; }
            set { output = value; }
        }

        [Parameter(Name = "LineMax", Description = "Maximum number of lines in files.")]
        public int FileLineMax
        {
            get { return fileLineMax; }
            set { fileLineMax = value; }
        }

        [Parameter(Name = "TimeMax", Description = "Maximum file length in time (in minutes).")]
        public int FileTimeMax
        {
            get { return fileTimeMax; }
            set { fileTimeMax = value; }
        }

        public SampleBase()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.compress = false;
            this.output = null;
            this.fileLineMax = -1;
            this.fileTimeMax = -1;
        }

        protected abstract TwitterLib.TwitterMethodBase GetMethod();

        public override void Run()
        {
            Console.Error.WriteLine("Starting stream, press Esc to stop...");

            TwitterMethodBase method = GetMethod();
            TwitterLiveStreamConsumer tls = new TwitterLiveStreamConsumer(method);

            // Intialize parameters
            tls.DumpCompressed = compress;
            tls.WriteDump = true;
            tls.DumpLocation = output != null ? output : "";
            tls.DumpLineMax = fileLineMax;
            tls.DumpTimeMax = fileTimeMax;

            tls.Open();

            DateTime start = DateTime.Now;
            int q = 0;
            while (true)
            {
                string line = tls.ReadLine();

                if ((DateTime.Now - start).TotalSeconds >= 5)
                {
                    Console.WriteLine(tls.GetStatistics());
                    start = DateTime.Now;
                }

                // DEBUG CODE
                // Use this to simulate stalling
                /*
                System.Threading.Thread.Sleep(100);
                */
                // END DEBUG CODE

                // Wait for exit key
                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey().Key == ConsoleKey.Escape)
                    {
                        break;
                    }
                }

                q++;
            }

            tls.Close();

            Console.WriteLine("Stream stopped.");
        }
    }
}
