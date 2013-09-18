using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace TwitterLib
{
    /// <summary>
    /// Implements base functionality to read twitter streams and dump them to disk.
    /// </summary>
    public abstract class TwitterConsumerBase
    {
        private bool inputCompressed;
        protected Stream inputStream;
        protected TwitterStreamReader inputStreamReader;

        private bool writeDump;
        private bool dumpCompressed;
        private string dumpLocation;
        private int dumpLineMax;
        private int dumpTimeMax;
        private DateTime dumpTimeStart;
        private int dumpLineCount;
        private Stream dumpStream;
        private GZipStream dumpZipStream;
        private StreamWriter dumpStreamWriter;

        private DateTime lastReceived;

        public bool InputCompressed
        {
            get { return inputCompressed; }
            set { inputCompressed = value; }
        }

        public bool WriteDump
        {
            get { return writeDump; }
            set { writeDump = value; }
        }

        public bool DumpCompressed
        {
            get { return dumpCompressed; }
            set { dumpCompressed = value; }
        }

        public string DumpLocation
        {
            get { return dumpLocation; }
            set { dumpLocation = value; }
        }

        public int DumpLineMax
        {
            get { return dumpLineMax; }
            set { dumpLineMax = value; }
        }

        public int DumpTimeMax
        {
            get { return dumpTimeMax; }
            set { dumpTimeMax = value; }
        }

        public TwitterConsumerBase()
        {
            InitializeMembers();
        }

        protected DateTime LastReceived
        {
            get { return lastReceived; }
        }

        private void InitializeMembers()
        {
            this.inputCompressed = false;
            this.inputStream = null;
            this.inputStreamReader = null;

            this.writeDump = false;
            this.dumpCompressed = true;
            this.dumpLocation = null;
            this.dumpLineMax = -1;
            this.dumpTimeMax = -1;
            this.dumpTimeStart = DateTime.MinValue;
            this.dumpLineCount = -1;
            this.dumpStream = null;
            this.dumpZipStream = null;
            this.dumpStreamWriter = null;

            this.lastReceived = DateTime.MinValue;
        }

        public abstract void Open();

        public virtual void Close()
        {
            if (this.inputStream == null)
            {
                throw new TwitterReaderException("No open stream.");
            }

            CloseDump();
        }

        public abstract string ReadLine();

        protected Dictionary<string, object> ProcessLine(string line, bool useSystemTime)
        {
            // Trim \n characters from the beginning
            // these are only used for keep-alive
            line = line.TrimStart('\n');

            // Check if the first character is not { (means partial message)
            if (!line.StartsWith("{"))
            {
                Console.Error.WriteLine("Skipping partial message.");

                return null;
            }

            // Parse message
            Dictionary<string, object> status = null;
            try
            {
                status = (Dictionary<string, object>)fastJSON.JSON.Instance.Parse(line);
            }
            catch (Exception e)
            {
                // This is a possible parsing error from a partial message
                Console.Error.WriteLine("JSON parser error: {0}", e.Message);

                return null;
            }

            // Check special messages
            if (status.ContainsKey("warning"))
            {
                // Use this to get info on stalling

                return null;
            }
            else if (status.ContainsKey("limit"))
            {
                // Use this to see filtered messages

                return null;
            }

            // Get time stamp
            DateTime now;
            
            if (useSystemTime)
            {
                now = DateTime.Now;
            }
            else if (status.ContainsKey("created_at"))
            {
                if (!Util.TryParseDateTime((string)status["created_at"], out now))
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                now = lastReceived;
            }

            lastReceived = now;

            // Dump status
            if (writeDump)
            {
                if (dumpStreamWriter == null)
                {
                    OpenDump(now);
                }

                dumpStreamWriter.Write(line);
                dumpStreamWriter.Write('\r');   // put back \r to make it a valid twitter stream

                this.dumpLineCount++;

                // Check if new dump file is to be created
                CheckDumpFull(now);
            }

            return status;
        }

        protected void OpenDump(DateTime now)
        {
            if (writeDump)
            {
                string filename = GetDumpFilename();

                if (dumpCompressed)
                {
                    filename += ".gz";
                    dumpStream = new FileStream(filename, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                    dumpZipStream = new GZipStream(dumpStream, CompressionMode.Compress);
                    dumpStreamWriter = new StreamWriter(dumpZipStream);
                }
                else
                {
                    dumpStreamWriter = new StreamWriter(filename);
                }

                Console.Error.WriteLine("Writing dump into: {0}", Path.GetFileName(filename));

                dumpTimeStart = now;
                dumpLineCount = 0;
            }
        }

        protected void CloseDump()
        {
            if (dumpStreamWriter != null)
            {
                dumpStreamWriter.Close();
                dumpStreamWriter.Dispose();
                dumpStreamWriter = null;
            }

            if (dumpZipStream != null)
            {
                dumpZipStream.Close();
                dumpZipStream.Dispose();
                dumpZipStream = null;
            }

            if (dumpStream != null)
            {
                dumpStream.Close();
                dumpStream.Dispose();
                dumpStream = null;
            }
        }

        protected void CheckDumpFull(DateTime now)
        {
            if ((dumpLineMax != -1 && dumpLineMax <= dumpLineCount) ||
                (dumpTimeMax != -1 && dumpTimeMax <= (now - dumpTimeStart).TotalMinutes))
            {
                CloseDump();
                OpenDump(now);
            }
        }

        protected abstract string GetDumpFilename();
    }
}
