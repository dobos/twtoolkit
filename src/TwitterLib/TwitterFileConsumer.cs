using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TwitterLib
{
    /// <summary>
    /// Implements a class that reads a twitter stream from a local file
    /// </summary>
    public class TwitterFileConsumer : TwitterConsumerBase
    {
        private string filename;
        private int fileCounter;

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        public TwitterFileConsumer()
            :base()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.filename = null;
            this.fileCounter = 0;

            this.inputStream = null;
        }

        public override void Open()
        {
            if (this.inputStream != null)
            {
                throw new TwitterReaderException("Stream already open.");
            }

            inputStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            inputStreamReader = new TwitterStreamReader(inputStream);
        }

        public override void Close()
        {
            base.Close();

            this.inputStream.Close();
            this.inputStream.Dispose();
        }

        public override string ReadLine()
        {
            if (inputStream == null)
            {
                throw new TwitterReaderException("No open stream.");
            }

            string line = null;

            while ((line = inputStreamReader.ReadLine()) != null)
            {
                Dictionary<string, object> status = ProcessLine(line, false);

                if (status != null)
                {
                    return line;
                }
            }

            return null;
        }

        protected override string GetDumpFilename()
        {
            fileCounter++;

            string res = String.Format(
                "{0}_{1}.txt",
                Path.GetFileNameWithoutExtension(filename),
                fileCounter);

            return Path.Combine(DumpLocation, res);
        }
    }
}
