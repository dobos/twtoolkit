using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterGeoLocation
{
    class WorkerArgs
    {
        public FoFClustering fof;
        public long userID;
        public List<GeoPoint> points;

        public WorkerArgs()
        { 
        }
    }
}
