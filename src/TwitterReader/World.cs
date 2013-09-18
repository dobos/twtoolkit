using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using TwitterLib;
using TwitterLib.CommandLineParser;

namespace TwitterReader
{
    [Verb(Name = "World", Description = "Dumps a twitter GPS coordinate stream into disk files.")]
    class World : SampleBase
    {
        protected override TwitterMethodBase GetMethod()
        {
            return new TwitterLib.Methods.World();
        }
    }
}
