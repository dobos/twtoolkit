using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterGeoLocation
{
    /// <summary>
    /// Represents spherical coordinates as a unit vector.
    /// </summary>
    struct GeoPoint
    {
        private double x;
        private double y;
        private double z;
        private double lon;
        private double lat;
        private DateTime time;

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public double Z
        {
            get { return z; }
            set { z = value; }
        }

        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }

        public double Lon
        {
            get
            {
                if (double.IsNaN(lon))
                {
                    Xyz2LonLat();
                }
                return lon;
            }
            set { lon = value; }
        }

        public double Lat
        {
            get
            {
                if (double.IsNaN(lat))
                {
                    Xyz2LonLat();
                }
                return lat;
            }
            set { lat = value; }
        }

        public void Normalize()
        {
            double length = Math.Sqrt((this.x * this.x) + (this.y * this.y) + (this.z * this.z));
            this.x = this.x / length;
            this.y = this.y / length;
            this.z = this.z / length;
        }

        public string ToString()
        {
            return "(" + x.ToString() + ";" + y.ToString() + ";" + z.ToString() + ")";
        }

        public GeoPoint(double cx, double cy, double cz)
        {
            this.x = cx;
            this.y = cy;
            this.z = cz;
            this.lon = double.NaN;
            this.lat = double.NaN;
            this.time = DateTime.MinValue;
        }

        private void Xyz2LonLat()
        {
            double epsilon = Constant.DoublePrecision2x;

            double rdec;
            if (z >= 1) rdec = Math.PI / 2;
            else if (z <= -1) rdec = -Math.PI / 2;
            else rdec = Math.Asin(z);
            lat = rdec * Constant.Radian2Degree;

            double cd = Math.Cos(rdec);
            if (cd > epsilon || cd < -epsilon)  // is the vector pointing to the poles?
            {
                if (y > epsilon || y < -epsilon)  // is the vector in the x-z plane?
                {
                    double arg = x / cd;
                    double acos;
                    if (arg <= -1)
                    {
                        acos = 180;
                    }
                    else if (arg >= 1)
                    {
                        acos = 0;
                    }
                    else
                    {
                        acos = Math.Acos(arg) * Constant.Radian2Degree;
                    }
                    if (y < 0.0)
                    {
                        lon = 360 - acos;
                    }
                    else
                    {
                        lon = acos;
                    }
                }
                else
                {
                    lon = (x < 0.0 ? 180.0 : 0.0);
                }
            }
            else
            {
                lon = 0.0;
            }

            if (lon < 180) lon = lon + 360;
            if (lon > 180) lon = lon - 360;
        }

        public double Angle(GeoPoint other)
        {
            double cosf = ((this.x * other.x) + (this.y * other.y) + (this.z * other.z));
            // TODO: make sure vectors are normalized
            // (Math.Sqrt((u.cx * u.cx) + (u.cy * u.cy) + (u.cz * u.cz)) * Math.Sqrt((v.cx * v.cx) + (v.cy * v.cy) + (v.cz * v.cz)));
            return Math.Acos(cosf) * Constant.Radian2Degree;
        }

        public static GeoPoint Avg(IEnumerable<GeoPoint> points)
        {
            double xx = 0;
            double yy = 0;
            double zz = 0;
            int count = 0;

            foreach (var p in points)
            {
                xx += p.X;
                yy += p.Y;
                zz += p.Z;
                count++;
            }

            xx /= count;
            yy /= count;
            zz /= count;

            GeoPoint r = new GeoPoint(xx, yy, zz);
            r.Normalize();

            return r;
        }

        public static double Sigma2(IEnumerable<GeoPoint> points, GeoPoint avg)
        {
            double sigma2 = 0;
            int count = 0;

            foreach (var p in points)
            {
                sigma2 += avg.Angle(p);
                count++;
            }

            sigma2 /= count;

            return sigma2;
        }
    }
}
