using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterLib.Load.Mergers
{
    class UserUpdate : Merger
    {
        protected override string SourceTableName
        {
            get { return "user"; }
        }

        protected override string TargetTableName
        {
            get { return "user_update"; }
        }
    }
}
