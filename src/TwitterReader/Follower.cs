using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitterLib;
using TwitterLib.CommandLineParser;

namespace TwitterReader
{
    [Verb(Name = "Follower", Description = "Gets the followers of a user.")]
    class Follower : Verb
    {
        private long userId;
        private string output;

        [Parameter(Name = "UserID", Description = "User ID.")]
        public long UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        [Parameter(Name = "Output", Description = "Output path.")]
        public string Output
        {
            get { return output; }
            set { output = value; }
        }

        public Follower()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.userId = -1;
            this.output = null;
        }

        public override void Run()
        {
            Console.Error.WriteLine("Starting stream, press Esc to stop...");

            var method = new TwitterLib.Methods.Follower();
            method.UserId = userId;

            TwitterLiveStreamConsumer tls = new TwitterLiveStreamConsumer(method);

            // Intialize parameters
            //tls.DumpCompressed = compress;
            tls.WriteDump = true;
            tls.DumpLocation = output != null ? output : "";
            //tls.DumpLineMax = fileLineMax;
            //tls.DumpTimeMax = fileTimeMax;

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
