using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using TwitterLib;
using TwitterLib.Logging;

namespace TwitterLib.Load
{
    public class Chunk
    {
        private int id;

        private int batchID;
        private string chunkID;

        private short runId;

        private DateTime prepareStart;
        private DateTime prepareEnd;
        private DateTime loadStart;
        private DateTime loadEnd;
        private DateTime mergeStart;
        private DateTime mergeEnd;
        private DateTime cleanupStart;
        private DateTime cleanupEnd;

        private string filename;
        private bool overlapped;
        private bool binary;
        private bool skip;
        private string bulkPath;

        private SqlConnectionStringBuilder targetDB;
        private SqlConnectionStringBuilder loaderDB;

        private List<Mapper> mappings;
        private List<Merger> mergers;

        /// <summary>
        /// Chunk ID as an auto increment integer
        /// </summary>
        /// <remarks>
        /// Used internally by the loader, not stored in admin DB
        /// </remarks>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public int BatchID
        {
            get { return batchID; }
            set { batchID = value; }
        }

        /// <summary>
        /// Chunk ID as generated from the input file name
        /// </summary>
        public string ChunkId
        {
            get { return chunkID; }
            set { chunkID = value; }
        }

        public short RunId
        {
            get { return runId; }
            set { runId = value; }
        }

        public DateTime PrepareStart
        {
            get { return prepareStart; }
            set { prepareStart = value; }
        }

        public DateTime PrepareEnd
        {
            get { return prepareEnd; }
            set { prepareEnd = value; }
        }

        public DateTime LoadStart
        {
            get { return loadStart; }
            set { loadStart = value; }
        }

        public DateTime LoadEnd
        {
            get { return loadEnd; }
            set { loadEnd = value; }
        }

        public DateTime MergeStart
        {
            get { return mergeStart; }
            set { mergeStart = value; }
        }

        public DateTime MergeEnd
        {
            get { return mergeEnd; }
            set { mergeEnd = value; }
        }

        public DateTime CleanupStart
        {
            get { return cleanupStart; }
            set { cleanupEnd = value; }
        }

        public DateTime CleanupEnd
        {
            get { return cleanupEnd; }
            set { cleanupEnd = value; }
        }

        public bool Binary
        {
            get { return binary; }
            set { binary = value; }
        }

        public bool Skip
        {
            get { return skip; }
            set { skip = value; }
        }

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        public bool Overlapped
        {
            get { return overlapped; }
            set { overlapped = value; }
        }

        public string BulkPath
        {
            get { return bulkPath; }
            set { bulkPath = value; }
        }

        public SqlConnectionStringBuilder TargetDB
        {
            get { return targetDB; }
        }

        public SqlConnectionStringBuilder LoaderDB
        {
            get { return loaderDB; }
        }

        public bool IsPrepareComplete
        {
            get { return prepareStart > DateTime.MinValue && prepareEnd > DateTime.MinValue; }
        }

        public bool IsLoadComplete
        {
            get { return loadStart > DateTime.MinValue && loadEnd > DateTime.MinValue; }
        }

        public bool IsMergeComplete
        {
            get { return mergeStart > DateTime.MinValue && mergeEnd > DateTime.MinValue; }
        }

        public bool IsCleanupComplete
        {
            get { return cleanupStart > DateTime.MinValue && cleanupEnd > DateTime.MinValue; }
        }

        public Chunk()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.id = 0;

            this.batchID = 0;
            this.chunkID = null;

            this.runId = 0;

            this.prepareStart = DateTime.MinValue;
            this.prepareEnd = DateTime.MinValue;
            this.loadStart = DateTime.MinValue;
            this.loadEnd = DateTime.MinValue;
            this.mergeStart = DateTime.MinValue;
            this.mergeEnd = DateTime.MinValue;
            this.cleanupStart = DateTime.MinValue;
            this.cleanupEnd = DateTime.MinValue;

            this.filename = null;
            this.overlapped = false;
            this.binary = true;
            this.bulkPath = null;

            this.targetDB = new SqlConnectionStringBuilder();
            this.loaderDB = new SqlConnectionStringBuilder();

            this.mappings = new List<Mapper>()
            {
                new Mappers.Tweet(),
                new Mappers.TweetRetweet(),
                new Mappers.User(),
                new Mappers.TweetUserMention(),
                new Mappers.TweetHashtag(),
                new Mappers.TweetUrl(),
                new Mappers.DeleteTweet(),
                new Mappers.ScrubGeo(),
            };

            mergers = new List<Merger>()
            {
                new Mergers.Tweet(),
                new Mergers.TweetRetweet(),
                new Mergers.User(),
                new Mergers.UserUpdate(),
                new Mergers.TweetUserMention(),
                new Mergers.UserHashtag(),
                new Mergers.TweetHashtag(),
                new Mergers.TweetUrl(),
            };

        }

        /// <summary>
        /// Checks if all files exist
        /// </summary>
        /// <returns></returns>
        public bool CheckFiles()
        {
            var res = true;

            foreach (var m in mappings)
            {
                res &= File.Exists(m.GetFilename(this));
            }

            return res;
        }

        /// <summary>
        /// Creates bulk insert files
        /// </summary>
        public void CreateFiles()
        {
            if (skip && IsPrepareComplete && CheckFiles() || skip && IsLoadComplete)
            {
                Console.WriteLine("{0} > Skip preparing: {1}", id, Path.GetFileName(filename));
                return;
            }

            var start = DateTime.Now;
            Console.WriteLine("{0} > Preparing: {1}", id, Path.GetFileName(filename));

            bool skipall = true;
            foreach (Mapper m in mappings)
            {
                m.Binary = binary;
                m.RunID = RunId;
                skipall &= m.Open(this, skip);
            }

            int q = 0;
            if (!skipall)
            {
                using (var reader = new TwitterStreamReader(filename, overlapped))
                {

                    //var objects = reader.AsParallel().WithMergeOptions(ParallelMergeOptions.NotBuffered).WithDegreeOfParallelism(Environment.ProcessorCount).Select(
                    var objects = reader.Select(
                        delegate(string status)
                        {
                            try
                            {
                                return (Dictionary<string, object>)fastJSON.JSON.Instance.Parse(status);
                            }
                            catch (Exception e)
                            {
                                // This is a possible parsing error from a partial message
                                Console.Error.WriteLine("JSON parser error: {0}", e.Message);
                                return null;
                            }
                        });


                    foreach (var obj in objects)
                    {
                        if (obj != null)
                        {
                            foreach (Mapper m in mappings)
                            {
                                if (!m.Skipped)
                                {
                                    m.Map(obj);
                                }
                            }
                            q++;

                            /*if (q % 1000 == 0)
                            {
                                Console.WriteLine(q);
                            }*/
                        }
                    }

                    foreach (Mapper m in mappings)
                    {
                        if (!m.Skipped)
                        {
                            m.Close();
                        }
                    }
                }
            }

            var end = DateTime.Now;
            Console.WriteLine("{0} > {1} rows written in {2} sec.", id, q, (end - start).TotalSeconds);

            // Save status
            prepareEnd = end;
        }

        /// <summary>
        /// Creates tables for bulk insert in loader db
        /// </summary>
        public void CreateTables()
        {
            if (skip && IsLoadComplete)
            {
                Console.WriteLine("{0} > Skip creating table: {1}", id, Path.GetFileName(filename));
                return;
            }

            using (SqlConnection cn = new SqlConnection(loaderDB.ConnectionString))
            {
                cn.Open();

                using (SqlTransaction tn = cn.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    foreach (Mapper m in mappings)
                    {
                        m.DropTable(this, cn, tn);
                        m.CreateTable(this, cn, tn);
                    }

                    tn.Commit();
                }
            }
        }

        /// <summary>
        /// Executes bulk insert
        /// </summary>
        public void RunBulkInsert()
        {
            if (skip && IsLoadComplete)
            {
                Console.WriteLine("{0} > Skip loading: {1}", id, Path.GetFileName(filename));
                return;
            }

            using (SqlConnection cn = new SqlConnection(loaderDB.ConnectionString))
            {
                cn.Open();

                foreach (Mapper m in mappings)
                {
                    using (SqlTransaction tn = cn.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        m.BulkInsert(this, cn, tn);

                        tn.Commit();
                    }

                    using (SqlTransaction tn = cn.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        m.CreateIndex(this, cn, tn);

                        tn.Commit();
                    }
                }
            }

            // Save status
            loadEnd = DateTime.Now;
        }

        /// <summary>
        /// Merges tables into main db
        /// </summary>
        public void MergeTables()
        {
            if (skip && IsMergeComplete)
            {
                Console.WriteLine("{0} > Skip merging: {1}", id, Path.GetFileName(filename));
                return;
            }

            using (SqlConnection cn = new SqlConnection(loaderDB.ConnectionString))
            {
                cn.Open();

                foreach (Merger m in mergers)
                {
                    m.RunID = runId;

                    using (SqlTransaction tn = cn.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        m.MergeTable(this, cn, tn);

                        tn.Commit();
                    }
                }
            }


            // Save status
            mergeEnd = DateTime.Now;
        }

        /// <summary>
        /// Drops loader tables
        /// </summary>
        public void DropTables()
        {
            using (SqlConnection cn = new SqlConnection(loaderDB.ConnectionString))
            {
                cn.Open();

                foreach (Mapper m in mappings)
                {
                    using (SqlTransaction tn = cn.BeginTransaction(IsolationLevel.ReadUncommitted))
                    {
                        m.DropTable(this, cn, tn);

                        tn.Commit();
                    }
                }
            }

            // Save status
            cleanupEnd = DateTime.Now;
        }

        /// <summary>
        /// Deletes bulk insert files
        /// </summary>
        public void DeleteFiles()
        {
            foreach (Mapper m in mappings)
            {
                m.Delete(this);
            }

            string dir = GetBulkDirectory();
            if (Directory.Exists(dir))
            {
                try
                {
                    System.IO.Directory.Delete(dir);
                }
                catch (Exception)
                {
                }
            }

            // Save status
            cleanupEnd = DateTime.Now;
        }

        public string GetBulkDirectory()
        {
            return System.IO.Path.Combine(bulkPath, chunkID);
        }

        public void DisableIndexes()
        {
            ToogleIndexes(false);
        }

        public void RebuildIndexes()
        {
            ToogleIndexes(true);
        }

        private void ToogleIndexes(bool op)
        {
            var ix = new List<string[]>();

            var sql =
@"SELECT t.name, i.name
FROM [{0}].sys.tables t
INNER JOIN [{0}].sys.indexes i
	ON t.object_id = i.object_id
WHERE i.type = 2;	-- NONCLUSTERED";

            sql = String.Format(sql, targetDB.InitialCatalog);

            using (var cn = new SqlConnection(targetDB.ConnectionString))
            {
                cn.Open();

                using (var cmd = new SqlCommand(sql, cn))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var i = new string[2];
                            i[0] = dr.GetString(0);
                            i[1] = dr.GetString(1);

                            ix.Add(i);
                        }
                    }
                }
            }

            // This is to be done one-by-one!

            foreach (var i in ix)
            {
                using (var cn = new SqlConnection(targetDB.ConnectionString))
                {
                    cn.Open();

                    sql = String.Format("ALTER INDEX {2} ON [{0}]..{1} {3}", targetDB.InitialCatalog, i[0], i[1], op ? "REBUILD" : "DISABLE");

                    var start = DateTime.Now;
                    Console.Write("{0} index: {1}.{2}...", op ? "Rebuilding" : "Disabling", targetDB.InitialCatalog, i[1]);

                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        cmd.CommandTimeout = 36000; // 10 hours, it can be very slow!

                        cmd.ExecuteNonQuery();
                    }

                    Console.WriteLine(" done in {0} sec.", (DateTime.Now - start).TotalSeconds);
                }
            }
        }
    }
}
