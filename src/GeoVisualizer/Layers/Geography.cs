using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.SqlServer.Types;

namespace Elte.GeoVisualizer.Lib.Layers
{
    public class Geography : Layer
    {
        private FillStyle fillStyle;
        private LineStyle lineStyle;

        public FillStyle FillStyle
        {
            get { return fillStyle; }
        }

        public LineStyle LineStyle
        {
            get { return lineStyle; }
        }

        public Geography()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.fillStyle = new FillStyle();
            this.lineStyle = new LineStyle();
        }

        public override void OnRender(RenderingContext context, object[] values)
        {
            RenderGeography(context, (SqlGeography)values[0]);
        }

        protected void RenderGeography(RenderingContext context, SqlGeography geo)
        {
            var numgeo = geo.STNumGeometries().Value;

            if (numgeo > 1)
            {
                for (int i = 0; i < numgeo; i++)
                {
                    RenderGeography(context, geo.STGeometryN(i + 1));  // indexed from 1!
                }
            }
            else
            {
                var type = geo.STGeometryType().Value;

                switch (type)
                {

                    case "Polygon":
                        RenderPolygon(context, geo);
                        break;
                    case "LineString":
                        RenderPolyline(context, geo);
                        break;
                    case "Point":
                        RenderPoint(context, geo);
                        break;
                    case "CircularString":
                    case "CompoundCurve":
                    case "CurvePolygon":
                    case "GeometryCollection":
                    case "MultiPoint":
                    case "MultiLineString":
                    case "MultiPolygon":
                    default:
                        //throw new NotImplementedException();    // TODO
                        break;
                }
            }
        }

        private void RenderPolyline(RenderingContext context, SqlGeography geo)
        {
            foreach (var pp in MapPoints(context, geo))
            {
                if (lineStyle.IsVisible)
                {
                    var pen = lineStyle.GetPen(context);
                    Graphics.DrawLines(pen, pp);
                }
            }
        }

        private void RenderPolygon(RenderingContext context, SqlGeography geo)
        {
            foreach (var pp in MapPoints(context, geo))
            {
                if (fillStyle.IsVisible)
                {
                    Graphics.FillPolygon(fillStyle.GetBrush(context), pp);
                }

                if (lineStyle.IsVisible)
                {
                    var pen = lineStyle.GetPen(context);
                    Graphics.DrawPolygon(pen, pp);
                }
            }
        }

        private void RenderPoint(RenderingContext context, SqlGeography geo)
        {
            var gp = new GeoPoint(geo.Long.Value, geo.Lat.Value);
            var mp = context.Projection.Map(gp);

            if (lineStyle.IsVisible)
            {
                Graphics.DrawLine(lineStyle.GetPen(context), (int)mp.X, (int)mp.Y, (int)mp.X + 1, (int)mp.Y + 1);
            }
        }

        private Point[][] MapPoints(RenderingContext context, SqlGeography geo)
        {
            var sink = new MapGeographySink(context.Projection);
            geo.Populate(sink);

            var res = new Point[sink.Shapes.Count][];

            for (int i = 0; i < sink.Shapes.Count; i++)
            {
                var pp = sink.Shapes[i];
                res[i] = new Point[pp.Length];
                for (int j = 0; j < pp.Length; j++)
                {
                    res[i][j] = new Point((int)pp[j].X, (int)pp[j].Y);
                }
            }

            return res;
        }
    }
}
