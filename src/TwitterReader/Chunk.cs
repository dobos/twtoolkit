using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using TwitterLib;
using TwitterLib.CommandLineParser;

namespace TwitterReader
{
    [Verb(Name = "Chunk", Description = "Chunks up a single saved stream.")]
    class Chunk : Verb
    {
        private bool compress;
        private string input;
        private string output;
        private int fileLineMax;
        private int fileTimeMax;

        [Option(Name = "Compress", Description = "Compress output stream.", Required = false)]
        public bool Compress
        {
            get { return compress; }
            set { compress = value; }
        }

        [Parameter(Name = "Input", Description = "Input file.", Required=true)]
        public string Input
        {
            get { return input; }
            set { input = value; }
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

        public Chunk()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.compress = true;
            this.input = null;
            this.output = null;
            this.fileLineMax = -1;
            this.fileTimeMax = -1;
        }

        public override void Run()
        {
            TwitterFileConsumer tf = new TwitterFileConsumer();

            // Intialize parameters
            tf.Filename = input;

            tf.DumpCompressed = true;
            tf.WriteDump = true;
            tf.DumpLocation = output != null ? output : "";
            tf.DumpLineMax = fileLineMax;
            tf.DumpTimeMax = fileTimeMax;

            tf.Open();

            DateTime start = DateTime.Now;

            string line;
            int q = 0;
            while ((line = tf.ReadLine()) != null)
            {
                if ((DateTime.Now - start).TotalSeconds >= 5)
                {
                    Console.WriteLine(q); //tf.GetStatistics());
                    start = DateTime.Now;
                }

                q++;
            }

            tf.Close();
        }
    }
}
