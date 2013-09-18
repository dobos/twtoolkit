using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterLib.Load.Mergers
{
    class User : Merger
    {
        protected override string SourceTableName
        {
            get { return "user"; }
        }

        protected override string TargetTableName
        {
            get { return "user"; }
        }
    }
}
