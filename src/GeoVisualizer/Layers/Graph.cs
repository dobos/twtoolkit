using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Elte.GeoVisualizer.Lib.Layers
{
    public class Graph : Layer
    {
        private LineStyle lineStyle;

        public LineStyle LineStyle
        {
            get { return lineStyle; }
        }

        public Graph()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.lineStyle = new LineStyle();
        }

        public override void OnRender(RenderingContext context, object[] values)
        {
            var gp1 = new GeoPoint((double)values[0], (double)values[1]);
            var gp2 = new GeoPoint((double)values[2], (double)values[3]);
            var dist = (double)values[4];

            var mp = context.Projection.Interpolate(gp1, gp2, 5);

            var points = new Point[mp.Length];
            for (int i = 0; i < mp.Length; i++)
            {
                points[i] = new Point((int)mp[i].X, (int)mp[i].Y);
            }

            using (var p = new Pen(Color.FromArgb(1, 255, 0, 0)))
            {

                Graphics.DrawCurve(p, points);
            }
        }
    }
}
