using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Elte.GeoVisualizer.Lib.Layers
{
    public class Histogram : Layer
    {
        private Color color;

        private Layer alpha;
        private double[,] hist;
        private double[,] kernel;

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public Layer Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        public override IEnumerable<Layer> Layers
        {
            get
            {
                if (alpha == null)
                {
                    yield break;
                }
                else
                {
                    yield return alpha;
                }
            }
        }

        public Histogram()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.color = Color.White;
        }

        public override void OnBeginRender(RenderingContext context)
        {
            base.OnBeginRender(context);

            hist = new double[context.Width, context.Height];

            kernel = new double[3, 3]
            { {0, 1, 0},
              {1, 3, 1},
              {0, 1, 0} };
        }

        public override void OnRender(RenderingContext context, object[] values)
        {
            double lon = (double)values[0];
            double lat = (double)values[1];

            var mp = context.Projection.Map(new GeoPoint(lon, lat));

            for (int ik = 0; ik < kernel.GetLength(0); ik++)
            {
                for (int jk = 0; jk < kernel.GetLength(1); jk++)
                {
                    int i = (int)mp.X + ik - (kernel.GetLength(0) - 1) / 2;
                    int j = context.Height - (int)mp.Y + jk - (kernel.GetLength(1) - 1) / 2;

                    if (i >= 0 && i < hist.GetLength(0) &&
                        j >= 0 && j < hist.GetLength(1))
                    {
                        hist[i, j] += kernel[ik, jk];
                    }
                }
            }
        }

        public override System.Drawing.Bitmap OnEndRender()
        {
            byte[] abuffer = null;
            int abytes;
            int astride;

            if (alpha != null)
            {
                alpha.OnEndRender();

                alpha.LockBits(out abuffer, out abytes, out astride);
                //alpha.UnlockBits();
            }

            double max = GetMax();

            byte[] buffer;
            int bytes;
            int stride;

            int b = 0;
            int g = 1;
            int r = 2;
            int a = 3;

            LockBits(out buffer, out bytes, out stride);

            for (int i = 0; i < hist.GetLength(0); i++)
            {
                for (int j = 0; j < hist.GetLength(1); j++)
                {
                    double v = Math.Log10(Math.Log10(hist[i, j] + 1) + 1);

                    int k = j * stride + i * bytes;

                    /*if (alpha != null)
                    {
                        buffer[k + a] = abuffer[k + a];
                    }*/

                    /*buffer[k + r] = (byte)((v / max) * color.R);
                    buffer[k + g] = (byte)((v / max) * color.G);
                    buffer[k + b] = (byte)((v / max) * color.B);*/

                    buffer[k + r] = (byte)color.R;
                    buffer[k + g] = (byte)color.G;
                    buffer[k + b] = (byte)color.B;

                    buffer[k + a] = (byte)((v / max) * color.A / 256.0 * abuffer[k + a]);
                }
            }

            UnlockBits();

            return base.OnEndRender();
        }

        private double GetMax()
        {
            double max = double.MinValue;

            for (int i = 0; i < hist.GetLength(0); i++)
            {
                for (int j = 0; j < hist.GetLength(1); j++)
                {
                    max = Math.Max(max, Math.Log10(Math.Log10(hist[i, j] + 1) + 1));
                }
            }

            return max;
        }
    }
}
