using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;

namespace TwitterLib
{
    /// <summary>
    /// Class to support stream keep-alive when HTTP connection is broken
    /// </summary>
    /// <remarks>
    /// Automatically reestablishes connection on error.
    /// Handles stall-warning messages.
    /// Calculates stream statistics
    /// </remarks>
    public class TwitterLiveStreamConsumer : TwitterConsumerBase
    {
        private TwitterMethodBase method;

        private bool handleStalling;
        private bool handleMissed;
        private bool autoReconnect;

        private HttpWebRequest request;
        private HttpWebResponse response;

        private DateTime totalStart;
        private long totalCount;
        private DateTime currentStart;
        private long currentCount;
        private double currentSpeed;
        private double maxSpeed;

        private int networkErrorBackOff;
        private int httpErrorBackOff;

        public TwitterLiveStreamConsumer(TwitterMethodBase method)
            : base()
        {
            InitializeMembers();

            this.method = method;
        }

        private void InitializeMembers()
        {
            this.method = null;

            this.handleStalling = true;
            this.handleMissed = false;      // TODO: this doesn't work on public stream!
            this.autoReconnect = true;

            this.request = null;
            this.request = null;

            this.totalStart = DateTime.Now;
            this.totalCount = 0;

            this.networkErrorBackOff = 250;
            this.httpErrorBackOff = 10;
        }

        public override string ReadLine()
        {
            if (inputStream == null)
            {
                throw new TwitterReaderException("No open stream.");
            }

            string line = null;

            while (true)
            {
                try
                {
                    line = inputStreamReader.ReadLine();

                    // If null is returned the stream has closed
                    if (line == null)
                    {
                        throw new TwitterReaderException("End of stream.");
                    }
                }
                catch (Exception ex)
                {
                    // This might mean a broken connection

                    HandleNetworkError(ex);
                    continue;
                }

                Dictionary<string, object> status = ProcessLine(line, true);

                if (status == null)
                {
                    continue;
                }

                // Check time stamp skew now and then
                /*
                if (totalCount % 1000 == 0)
                {
                    if (status.ContainsKey("created_at"))
                    {
                        DateTime stamp;

                        if (Util.TryParseDateTime((string)status["created_at"], out stamp))
                        {
                            if (Math.Abs((DateTime.Now - stamp.ToLocalTime()).TotalSeconds) > 10)
                            {
                                // TODO: Add code here to handle large skew which would
                                // lead to stalling
                            }
                        }
                    }
                }
                 * */

                break;
            }

            // Update statistics
            UpdateStatistics();

            // DEBUG CODE
            // Simulate broken connection
            /*
            if (totalCount % 2000 == 0)
            {
                request.Abort();
            }
            */
            // END DEBUG CODE

            return line;
        }

        public override void Open()
        {
            if (this.inputStream != null)
            {
                throw new TwitterReaderException("Stream already open.");
            }

            // Mono requires this line to open SSL connections
            System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate
            {
                return true;
            };

            try
            {
                if (method is TwitterStreamingMethodBase)
                {
                    HandleMissed();
                    ((TwitterStreamingMethodBase)method).Delimited = false;
                    ((TwitterStreamingMethodBase)method).StallWarnings = handleStalling;
                }
                else if (method is TwitterCursoredMethodBase)
                {
                }

                this.request = method.GetHttpWebRequest();
                Console.Error.WriteLine("Connecting to: {0}", request.Address.ToString());

                // Set up compression
                if (this.InputCompressed)
                {
                    this.request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                    this.request.Headers.Add("Accept-Encoding", "deflate, gzip");
                }

                // Send request and get response
                this.response = (HttpWebResponse)this.request.GetResponse();

                this.inputStream = this.response.GetResponseStream();
                this.inputStreamReader = new TwitterStreamReader(this.inputStream);

                // Restart statistics counters
                StartStatistics();

                // Reset back off variables on successful connect
                this.networkErrorBackOff = 250;
                this.httpErrorBackOff = 10;
            }
            catch (WebException ex)
            {
                HandleHttpError(ex);
            }
            catch (Exception ex)
            {
                HandleNetworkError(ex);
            }
        }

        private void HandleMissed()
        {
            if (method is TwitterStreamingMethodBase && handleMissed && totalCount > 0)
            {
                int count = 0;

                double missing = 2 * ((DateTime.Now - LastReceived).TotalSeconds + 5) * maxSpeed;

                count = (int)Math.Ceiling(missing);

                if (count > 150000)
                {
                    Console.Error.WriteLine("Too many messages missing.");

                    count = 150000;
                }

                // DEBUG CODE
                // retrieve missed statuses only

                // count *= -1;

                // END DEBUG CODE

                Console.Error.WriteLine("Repeating {0} statuses.", count);

                ((TwitterStreamingMethodBase)method).Count = count;
            }
        }

        public override void Close()
        {
            base.Close();

            // Abort request
            this.request.Abort();

            // Close and dispose streams
            if (response != null)
            {
                this.response.Close();
            }

            if (inputStream != null)
            {
                this.inputStream.Close();
                this.inputStream.Dispose();
            }

            // Reset variables
            this.request = null;
            this.response = null;
            this.inputStream = null;
        }

        private void HandleNetworkError(Exception ex)
        {
            Close();

            Console.Error.WriteLine("Network error: {0}", ex.Message);
            Console.Error.WriteLine(ex.StackTrace);

            if (autoReconnect && networkErrorBackOff < 16000)
            {
                // Sleep and try to reconnect
                System.Threading.Thread.Sleep(networkErrorBackOff);
                networkErrorBackOff += 250;

                Console.Error.WriteLine("Reconnecting...");

                Open();
            }
            else
            {
                throw ex;
            }
        }

        private void HandleHttpError(Exception ex)
        {
            Console.Error.WriteLine("HTTP error: {0}", ex.Message);
            Console.Error.WriteLine(ex.StackTrace);

            if (autoReconnect)
            {
                // Sleep and try to reconnect
                System.Threading.Thread.Sleep(httpErrorBackOff * 1000);
                if (httpErrorBackOff < 64)
                {
                    httpErrorBackOff *= 2;
                }

                Console.Error.WriteLine("Reconnecting...");

                Open();
            }
            else
            {
                throw ex;
            }
        }

        private void StartStatistics()
        {
            this.currentStart = DateTime.Now;
            this.currentCount = 0;
            this.currentSpeed = 0;
            this.maxSpeed = 0;
        }

        private void UpdateStatistics()
        {
            this.totalCount++;
            this.currentCount++;

            // Current stat
            TimeSpan elapsed = DateTime.Now - currentStart;
            if (elapsed.TotalSeconds > 0.5)
            {
                this.currentSpeed = this.currentCount / elapsed.TotalSeconds;
                this.maxSpeed = Math.Max(this.maxSpeed, this.currentSpeed);

                this.currentCount = 0;
                this.currentStart = DateTime.Now;
            }
        }

        public string GetStatistics()
        {
            // Total stat
            double totalspeed = this.totalCount / (DateTime.Now - this.totalStart).TotalSeconds;
            return String.Format("Total: {0} @ {1:F2} status/sec; Current: {2:F2} status/sec.", this.totalCount, totalspeed, this.currentSpeed);
        }

        protected override string GetDumpFilename()
        {
            string filename = String.Format("{0}_{1:yyMMdd_HHmmss}.txt", method.GetType().Name, DateTime.Now);

            return Path.Combine(DumpLocation, filename);
        }
    }
}
