using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterGeoLocation
{
    class FoFClustering
    {
        private List<GeoCluster> clusters;

        public List<GeoCluster> Clusters
        {
            get { return clusters; }
        }

        public FoFClustering()
        {
        }

        private void InitializeMembers()
        {
            this.clusters = null;
        }

        private bool IsFriend(GeoPoint a, GeoPoint b)
        {
            return a.Angle(b) < 0.00898;    // TODO: pull out constant as property
        }

        /// <summary>
        /// Finds FoF clusters in a list of points
        /// </summary>
        /// <param name="points"></param>
        public void FindClusters(IEnumerable<GeoPoint> points)
        {
            // This will hold results
            clusters = new List<GeoCluster>();

            // A hash set will contain all points not in a cluster yet
            var input = new HashSet<GeoPoint>(points);
            var temp = new HashSet<GeoPoint>();

            int k = 1;

            GeoPoint actual = new GeoPoint();
            GeoPoint tempActual = new GeoPoint();


            while (input.Count > 0)
            {
                var gclust = new GeoCluster();
                gclust.ClusterId = k;

                actual = input.ElementAt(0);
                input.Remove(actual);

                temp.Add(actual);

                while (temp.Count != 0)
                {
                    tempActual = temp.ElementAt(0);
                    gclust.Points.Add(tempActual);
                    temp.Remove(tempActual);

                    foreach (GeoPoint g in input)
                    {
                        if (IsFriend(g, tempActual))
                        {
                            if (!temp.Contains(g))
                            {
                                temp.Add(g);
                            }
                        }
                    }

                    foreach (GeoPoint g in temp)
                    {
                        input.Remove(g);
                    }
                }

                clusters.Add(gclust);
                k++;
            }
        }

        /// <summary>
        /// Reduces a list of clusters.
        /// </summary>
        public void ReduceClusters()
        {
            var all = new List<GeoCluster>();

            foreach (var cluster in clusters)
            {
                cluster.Reduce();
                all.Add(cluster);
            }

            var q = from c in all
                    where c.FinalPointCount > 0
                    orderby c.FinalPointCount descending
                    select c;

            var res = new List<GeoCluster>();

            foreach(var c in q)
            {
                c.ClusterId = res.Count;
                res.Add(c);
            }

            clusters = res;
        }
    }
}
