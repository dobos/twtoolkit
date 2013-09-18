using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TwitterGeoLocation
{
    class GeoCluster
    {
        private long userId;
        private int clusterId;
        private int initialPointCount;
        private int finalPointCount;
        private int trimmingIter;
        private List<GeoPoint> points;
        private GeoPoint center;
        private double sigma;
        private int dayCount;
        private int nightCount;

        public long UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public int ClusterId
        {
            get { return clusterId; }
            set { clusterId = value; }
        }

        public int InitialPointCount
        {
            get { return initialPointCount; }
        }

        public int FinalPointCount
        {
            get { return finalPointCount; }
        }

        public int TrimmingIter
        {
            get { return trimmingIter; }
        }

        public List<GeoPoint> Points
        {
            get { return points; }
        }

        public GeoPoint Center
        {
            get { return center; }
        }

        public double Sigma
        {
            get { return sigma; }
        }

        public int DayCount
        {
            get { return dayCount; }
        }

        public int NightCount
        {
            get { return nightCount; }
        }

        public GeoCluster()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.userId = 0;
            this.clusterId = 0;
            this.initialPointCount = 0;
            this.finalPointCount = 0;
            this.trimmingIter = 0;
            this.points = new List<GeoPoint>();
            this.center = new GeoPoint();
            this.sigma = 0;
            this.dayCount = 0;
            this.nightCount = 0;
        }

        /// <summary>
        /// Throws away points outside 3 sigma
        /// </summary>
        private void TrimPoints()
        {
            var trimmed = new List<GeoPoint>();
            foreach (var p in points)
            {
                if (center.Angle(p) < 3.0 * sigma)
                {
                    trimmed.Add(p);
                }
            }

            points = trimmed;
        }

        /// <summary>
        /// Trims points in cluster to 3 sigma and calculates statistics
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public void Reduce()
        {
            initialPointCount = points.Count;

            int count;

            if (initialPointCount != 1)
            {

                // Trim points until all in 3 sigma or too many iterations
                int q = 0;
                do
                {
                    // Get average and sigma
                    center = GeoPoint.Avg(points);
                    sigma = Math.Sqrt(GeoPoint.Sigma2(points, center));
                    count = points.Count;

                    TrimPoints();
                    q++;
                }
                while (points.Count == count && q < 3);

                finalPointCount = points.Count;
                trimmingIter = q;
            }
            else 
            {
                center = points[0];
                finalPointCount = initialPointCount;
                trimmingIter = 0;
            }

            // TODO: add code to calculate day and night
            // make it a function call!
            CountDays();

        }

        private void CountDays()
        {
            foreach (var p in points)
            {
                if (p.Time.Hour > 8 && p.Time.Hour < 20)
                {
                    dayCount++;
                }
                else 
                {
                    nightCount++;
                }
            }
        }

#if false
        // TODO: old code, delete
        private int _clusterId;
        private GeoPoint _center;
        private DateTime _createdAt;
        private int _dayTime;
        private int _dayCount;
        private int _nightCount;

        /// <summary>
        /// Number of coordinates at daytime
        /// </summary>
        public int DayCount
        {
            get { return _dayCount; }
            set { _dayCount = value; }
        }

        /// <summary>
        /// Number of coordinates at nighttime
        /// </summary>
        public int NightCount
        {
            get { return _nightCount; }
            set { _nightCount = value; }
        }

        public int ClusterId
        {
            get { return _clusterId; }
            set { _clusterId = value; }
        }

        public int DayTime
        {
            get { return _dayTime; }
            set { _dayTime = value; }
        }

        public DateTime CreatedAt
        {
            get { return _createdAt; }
            set { _createdAt = value; }
        }

        public GeoCluster()
        {
            InitialiteMembers();
        }

        public GeoCluster(double x, double y, double z)
        {
            InitialiteMembers();

            _center.X = x;
            _center.Y = y;
            _center.Z = z;
        }

        public GeoCluster(GeoCluster old)
        {
            CopyMembers(old);
        }

        private void InitialiteMembers()
        {
            this._clusterId = -1;
            this._center = new GeoPoint();
            this._createdAt = DateTime.MinValue;
            this._dayTime = -1;
            this._dayCount = -1;
            this._nightCount = -1;
            this._nightCount = -1;
        }

        private void CopyMembers(GeoCluster old)
        {
            this._clusterId = old._clusterId;
            this._center = old._center;
            this._createdAt = old._createdAt;
            this._dayTime = old._dayTime;
            this._dayCount = old._dayCount;
            this._nightCount = old._nightCount;
            this._clusterId = old._clusterId;
        }
#endif
    }
}
