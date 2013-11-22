using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using TwitterLib.CommandLineParser;
using TwitterLib.Load;

using System.Collections.Concurrent;
using System.Threading;
using System.ComponentModel;
using System.Threading.Tasks;

namespace TwitterGeoLocation
{
    [Verb(Name = "Cluster", Description = "Execute clustering algorithm")]
    class Cluster : VerbBase
    {
        private string output;
        private int threads;
        private bool binary;

        private Stream outputStream;
        private BinaryWriter outputBinary;
        private TextWriter outputWriter;
        private BulkFileWriter bulkWriter;

        BlockingCollection<WorkerArgs> inputCollection = new BlockingCollection<WorkerArgs>(100);
        BlockingCollection<WorkerArgs> outputCollection = new BlockingCollection<WorkerArgs>(100);

        [Parameter(Name = "Output", Description = "Output file name.", Required = true)]
        public string Output
        {
            get { return output; }
            set { output = value; }
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

        private BulkFileWriter BulkWriter
        {
            get { return bulkWriter; }
        }

        public Cluster()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.output = null;
            this.threads = Environment.ProcessorCount;
            this.binary = false;
        }

        public override void Run()
        {
            string sql = @"
SELECT t.user_id, t.cx, t.cy, t.cz,
       DATEADD(second, t.utc_offset, t.created_at)
FROM [{0}].[dbo].[tweet] AS t
WHERE t.run_id = @run_id 
      AND t.lat IS NOT NULL 
      AND t.lon IS NOT NULL
ORDER BY t.run_id, t.user_id";

            sql = String.Format(sql, TwitterDb);

            long itercount=0;

            //-----tasks (n worker, 1 writer)
            var tasks = new Task[threads]; //INIT THREADS
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Factory.StartNew(TaskWorker);
            }

            var wtask = Task.Factory.StartNew(TaskWriter);
            //-----

            var cstr = GetConnectionString();
            Console.WriteLine("Connecting to: {0}", cstr);

            using (var cn = new SqlConnection(cstr))
            {
                cn.Open();
                Console.WriteLine("Connection open.");

                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.CommandTimeout = 3600;
                    cmd.Parameters.Add("@run_id", SqlDbType.SmallInt).Value = RunId;

                    using (var dr = cmd.ExecuteReader())
                    {

                        OpenOutput();

                        // Advance to the first record
                        if (dr.Read())
                        {
                            long userId;
                            List<GeoPoint> points;
                            while (ReadUser(dr, out userId, out points))
                            {
                                itercount++;
                                if (itercount % 10000 == 0)
                                {
                                    Console.WriteLine("{0}, {1}, {2}", itercount, userId, points.Count);
                                }
                                WorkerArgs wa = new WorkerArgs();
                                wa.points = points;
                                wa.userID = userId;

                                inputCollection.TryAdd(wa, -1);
                                /*
                                var cl = new FoFClustering();
                                cl.FindClusters(points);
                                cl.ReduceClusters();
                                */
                                  
                                // Take first n clusters and write out
                                /*foreach (var c in cl.Clusters.Take(3))  // TODO: pull out constant as parameter
                                {
                                    c.UserId = userId;
                                    WriteCluster(c);
                                }
                                */
                            }

                            inputCollection.CompleteAdding();
                            Task.WaitAll(tasks);
                            outputCollection.CompleteAdding();
                            wtask.Wait();
                        }

                        CloseOutput();
                    }
                }
            }
        }

        /// <summary>
        /// Read all tweets of the user and leave dr pointer on the first tweet of the upcoming one
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="userId"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        private bool ReadUser(SqlDataReader dr, out long userId, out List<GeoPoint> points)
        {
            userId = dr.GetInt64(0);
            points = new List<GeoPoint>();

            do
            {
                if (userId != dr.GetInt64(0))
                {
                    return true;
                }

                var point = new GeoPoint(dr.GetDouble(1), dr.GetDouble(2), dr.GetDouble(3));
                point.Time = dr.IsDBNull(4) ? DateTime.MinValue : dr.GetDateTime(4);
                points.Add(point);
            }
            while (dr.Read());

            return false;
        }

        private void OpenOutput()
        {

            outputStream = new FileStream(output, FileMode.Create, FileAccess.Write, FileShare.None, Constants.WriteBufferSize);

            if (binary)
            {
                outputBinary = new BinaryWriter(outputStream);
                bulkWriter = new BulkFileWriter(outputBinary);
            }
            else
            {
                outputWriter = new StreamWriter(outputStream, Encoding.Unicode);
                bulkWriter = new BulkFileWriter(outputWriter);
            }

        }

        private void CloseOutput()
        {
            if (binary)
            {
                outputBinary.Flush();
                outputBinary.Close();
                outputBinary.Dispose();
                outputBinary = null;
            }
            else
            {
                outputWriter.Flush();
                outputWriter.Close();
                outputWriter.Dispose();
                outputWriter = null;
            }
        }

        private void WriteCluster(GeoCluster cluster)
        {
            // [run_id] [smallint] NOT NULL
            BulkWriter.WriteSmallInt(RunId);

            // [user_id] [bigint] NOT NULL		
            BulkWriter.WriteBigInt(cluster.UserId);

            // [cluster_id] [tinyint] NOT NULL
            BulkWriter.WriteTinyInt((sbyte)cluster.ClusterId);

            // [location_count] [int] NOT NULL
            BulkWriter.WriteInt(cluster.InitialPointCount);

            // [location_count_trimmed] [int] NOT NULL
            BulkWriter.WriteInt(cluster.FinalPointCount);

            // [sigma] [float] NOT NULL
            BulkWriter.WriteFloat(cluster.Sigma);

            // [lat] [float] NOT NULL
            BulkWriter.WriteFloat(cluster.Center.Lat);

            // [lon] [float] NOT NULL
            BulkWriter.WriteFloat(cluster.Center.Lon);
            
            // [cx] [float] NOT NULL
            BulkWriter.WriteFloat(cluster.Center.X);

            // [cy] [float] NOT NULL
            BulkWriter.WriteFloat(cluster.Center.Y);

            // [cz] [float] NOT NULL
            BulkWriter.WriteFloat(cluster.Center.Z);

            // [htm_id] [bigint] NOT NULL
            BulkWriter.WriteFloat(0);       // TODO

            // [is_day] [bit] NULL
            BulkWriter.WriteNullableBit(cluster.DayCount > cluster.NightCount);     // TODO: verify
            
            // [day_count] [int] NOT NULL
            BulkWriter.WriteInt(cluster.DayCount);

            // [night_count] [int] NOT NULL
            BulkWriter.WriteInt(cluster.NightCount);

            // [iterations] [tinyint] NOT NULL
            BulkWriter.WriteTinyInt((sbyte)cluster.TrimmingIter);

            BulkWriter.EndLine();
        }

        private void TaskWorker()
        {
            while (!inputCollection.IsCompleted)
            {
                var wa = new WorkerArgs();
                List<GeoPoint> gpl = new List<GeoPoint>();
                if (inputCollection.TryTake(out wa, -1))
                {
                    var cl = new FoFClustering();
                    cl.FindClusters(wa.points);
                    cl.ReduceClusters();
                    wa.fof = cl;
                }

                outputCollection.TryAdd(wa,-1);
            }
        }

        private void TaskWriter()
        {
            while (!outputCollection.IsCompleted)
            {
                var wa = new WorkerArgs();
                if (outputCollection.TryTake(out wa, -1))
                {
                    foreach (var c in wa.fof.Clusters.Take(3))  // TODO: pull out constant as parameter
                    {
                        c.UserId = wa.userID;
                        WriteCluster(c);
                    }  
                }
            }
        }
    }
}
