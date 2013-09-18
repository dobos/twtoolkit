using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using TwitterLib;
using TwitterLib.CommandLineParser;

namespace TwitterReader
{
    [Verb(Name = "Sample", Description = "Dumps a twitter sample stream into disk files.")]
    class Sample : SampleBase
    {
        protected override TwitterMethodBase GetMethod()
        {
            return new TwitterLib.Methods.Sample();
        }
    }
}
