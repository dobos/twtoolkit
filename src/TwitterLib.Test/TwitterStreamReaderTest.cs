using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TwitterLib.Test
{
    [TestClass]
    public class TwitterStreamReaderTest
    {

        private IEnumerable<Dictionary<string, object>> Enumerate()
        {
            var path = @"C:\Data0\dobos\project\TwitterToolkit\bin\Sample_121001_145811.txt.gz";

            var reader = new TwitterStreamReader(path);

            foreach (var line in reader)
            {
                Dictionary<string, object> status = null;

                try
                {
                    status = (Dictionary<string, object>)fastJSON.JSON.Instance.Parse(line);
                }
                catch (Exception e)
                {
                    // This is a possible parsing error from a partial message
                    Console.Error.WriteLine("JSON parser error: {0}", e.Message);
                    continue;
                }

                yield return status;
            }
        }

        [TestMethod]
        public void SingleThreadedRead()
        {
            int i = 0;
            foreach (var status in Enumerate())
            {
                i++;
            }

            Assert.AreEqual(29996, i);
        }

        [TestMethod]
        public void MultiThreadedRead()
        {
            int i = 0;
            foreach (var status in Enumerate().AsParallel().WithMergeOptions(ParallelMergeOptions.NotBuffered))
            {
                i++;
            }

            Assert.AreEqual(29996, i);
        }


    }
}
