using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterLib.Methods
{
    public class World : Filter
    {
        protected override string GetPostData()
        {
            return "locations=-180,0,0,90,0,0,180,90,-180,-90,0,0,0,-90,180,0";
        }
    }
}
