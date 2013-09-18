using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterLib.Load.Mergers
{
    class Tweet : Merger
    {
        protected override string SourceTableName
        {
            get { return "tweet"; }
        }

        protected override string TargetTableName
        {
            get { return "tweet"; }
        }
    }
}
