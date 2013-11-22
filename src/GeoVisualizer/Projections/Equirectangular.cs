using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elte.GeoVisualizer.Lib.Projections
{
    public class Equirectangular : Projection
    {
        public override MapPoint OnMap(GeoPoint gp)
        {
            return new MapPoint(gp.Lon, gp.Lat);
        }
    }
}
