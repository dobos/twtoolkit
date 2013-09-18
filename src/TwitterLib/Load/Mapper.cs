using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace TwitterLib.Load
{
    public abstract class Mapper
    {
        private bool skipped;
        private bool binary;

        private FileStream outputStream = null;
        private BinaryWriter outputBinary = null;
        private TextWriter outputWriter = null;

        private BulkFileWriter bulkWriter = null;

        public bool Skipped
        {
            get { return skipped; }
        }

        public bool Binary
        {
            get { return binary; }
            set { binary = value; }
        }

        protected BulkFileWriter BulkWriter
        {
            get { return bulkWriter; }
        }

        public short RunID { get; set; }

        protected abstract string TableName { get; }

        public bool Open(Chunk chunk, bool skip)
        {
            string dir = chunk.GetBulkDirectory();
            Directory.CreateDirectory(dir);

            string filename = GetFilename(chunk);

            if (skip && File.Exists(filename))
            {
                outputWriter = null;
                skipped = true;
            }
            else
            {
                outputStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None, Constants.WriteBufferSize);

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

                skipped = false;
            }

            return skipped;
        }

        public void Close()
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

            /*outputStream.Flush();
            outputStream.Close();
            outputStream.Dispose();
            outputStream = null;*/
        }

        public void Delete(Chunk chunk)
        {
            string fn = GetFilename(chunk);
            if (File.Exists(fn))
            {
                File.Delete(fn);
            }
        }


        public string GetFilename(Chunk chunk)
        {
            var filename = Path.Combine(chunk.GetBulkDirectory(), TableName);
            filename += binary ? ".dat" : ".txt";

            return filename;
        }

        public abstract void Map(Dictionary<string, object> obj);

        

        private StringBuilder GetScript(string id)
        {
            try
            {
                PropertyInfo p = typeof(LoadScripts).GetProperty(id, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.IgnoreCase);
                return new StringBuilder((string)p.GetValue(null, null));
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Error scripting {0}.", id), ex);
            }
        }

        public void CreateTable(Chunk chunk, SqlConnection cn, SqlTransaction tn)
        {
            // Figure out script name to use
            StringBuilder sql = GetScript(String.Format("create_{0}", TableName));
            ReplaceNames(sql, chunk);

            using (SqlCommand cmd = new SqlCommand(sql.ToString(), cn, tn))
            {
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("{0} > Created table: {1}...", chunk.ID, TableName);
        }

        public void BulkInsert(Chunk chunk, SqlConnection cn, SqlTransaction tn)
        {
            DateTime start = DateTime.Now;
            Console.WriteLine("{0} > Loading table: {1}...", chunk.ID, TableName);

            try
            {

                // Figure out script name to use
                StringBuilder sql = new StringBuilder(binary ? LoadScripts.bulkinsert_binary : LoadScripts.bulkinsert);
                ReplaceNames(sql, chunk);

                using (SqlCommand cmd = new SqlCommand(sql.ToString(), cn, tn))
                {
                    cmd.CommandTimeout = 3600;
                    cmd.ExecuteNonQuery();
                }

                var end = DateTime.Now;
                Console.WriteLine("{0} >     ... loaded {1} in {2} sec.", chunk.ID, TableName, (end - start).TotalSeconds);

                // Save status
                chunk.LoadStart = chunk.LoadStart == DateTime.MinValue ? start : chunk.LoadStart;
                chunk.PrepareEnd = end;
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} >     ... error in loading {1}.", chunk.ID, TableName);
                Console.WriteLine("{0} > {1}", chunk.ID, ex.Message);
                throw;
            }
        }

        public void CreateIndex(Chunk chunk, SqlConnection cn, SqlTransaction tn)
        {
            // Figure out script name to use
            StringBuilder sb = GetScript(String.Format("index_{0}", TableName));
            ReplaceNames(sb, chunk);

            var sql = sb.ToString();

            if (!String.IsNullOrWhiteSpace(sql))
            {

                DateTime start = DateTime.Now;
                Console.WriteLine("{0} > Creating index on table: {1}...", chunk.ID, TableName);

                using (SqlCommand cmd = new SqlCommand(sql, cn, tn))
                {
                    cmd.CommandTimeout = 3600;
                    cmd.ExecuteNonQuery();
                }

                Console.WriteLine("{0} >     ... index created on {1} in {2} sec.", chunk.ID, TableName, (DateTime.Now - start).TotalSeconds);
            }
        }

        public void DropTable(Chunk chunk, SqlConnection cn, SqlTransaction tn)
        {
            var sql = new StringBuilder(LoadScripts.drop_table);
            ReplaceNames(sql, chunk);

            using (SqlCommand cmd = new SqlCommand(sql.ToString(), cn, tn))
            {
                cmd.CommandTimeout = 3600;
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("{0} > Dropped table: {1}...", chunk.ID, TableName);
        }

        private void ReplaceNames(StringBuilder sql, Chunk chunk)
        {
            sql.Replace("$dbname", chunk.LoaderDB.InitialCatalog);
            sql.Replace("$tablename", String.Format("{0}_{1}", chunk.ChunkId, TableName));
            sql.Replace("$ixname", String.Format("IX_{0}_{1}", chunk.ChunkId, TableName));
            sql.Replace("$filename", GetFilename(chunk));
        }
    }
}
