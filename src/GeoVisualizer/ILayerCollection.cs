using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elte.GeoVisualizer.Lib
{
    public interface ILayerCollection
    {
        IEnumerable<Layer> Layers { get; }
    }
}
