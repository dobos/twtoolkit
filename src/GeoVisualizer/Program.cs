using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.SqlServer.Types;
using System.Data.SqlTypes;

namespace Elte.GeoVisualizer.Lib
{
    class Program
    {
        const string cstr = "Data Source=RETDB02;Initial Catalog=Gadm;Integrated Security=true";

        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture =
                System.Globalization.CultureInfo.InvariantCulture;

            var map = new Map();

            //map.Projection = new Projections.Equirectangular();
            map.Projection = new Projections.Stereographic();
            //map.Projection.MapScale = new MapPoint(3000, 3000); // USA
            map.Projection.MapScale = new MapPoint(400000, 400000); // NY
            //map.Projection.GeoOrigin = new GeoPoint(-96, 40); // USA
            //map.Projection.GeoOrigin = new GeoPoint(-73.911, 40.74886); // NY
            map.Projection.GeoOrigin = new GeoPoint(-106.8, 35); // Albuq
            map.Projection.MapOffset = new MapPoint(640, 640);
            //map.Projection.ZoomTo(-125, -64, 24, 52);   // NY
            map.Projection.ZoomTo(-107.25, -106.2, 34.5, 35.6); //Albuqu
            map.Projection.MapMin = new MapPoint(0, 0);
            map.Projection.MapMax = new MapPoint(1280, 1280);

            
            


            var geo = new Layers.Geography();
            geo.FillStyle.Method = FillStyle.FillMethod.Constant;
            geo.FillStyle.Constant = new SolidBrush(Color.Black);
            geo.LineStyle.Method = LineStyle.DrawMethod.Empty;
            geo.LineStyle.Constant = Pens.LightGray;

            //map.Layers.Add(layer);

            //var sql = "SELECT geom.Reduce(1000) FROM Gadm..Region WHERE ID = 3426	-- Maryland";
            //var sql = "SELECT geom.Reduce(10) FROM Gadm..Region WHERE GeoLevel = 1 AND ISO = 'USA'";
            var sql = "SELECT geom.Reduce(10) FROM Gadm..Region WHERE GeoLevel = 2 AND ParentID = 3437";    // NM
            //var sql = "SELECT TOP 1 geom FROM Gadm..Region WHERE Name = 'Oregon'";

            geo.DataSource = new DataSources.SqlQuery()
                {
                    ConnectionString = cstr,
                    Command = new SqlCommand(sql)
                };

            

            //sql = "SELECT TOP 1000000 lon, lat FROM Twitter..tweet WHERE run_id = 1004 AND lon IS NOT NULL AND Lon BETWEEN -125 AND -64 AND Lat BETWEEN 24 AND 52";
            //sql = "SELECT TOP 1000000 lon, lat FROM jszule..user_location_partly WHERE run_id = 1004 AND lon IS NOT NULL AND Lon BETWEEN -125 AND -64 AND Lat BETWEEN 24 AND 52 AND cluster_id = 0";

            /*sql = @"select t.lon, t.lat
from [jszule].[dbo].[user_location_partly] as t
where t.lon > -74.53  and t.lon < -73.22
and t.lat > 40.49 and t.lat < 41.06 and t.is_night=0
";*/

            sql = @"select t.lon, t.lat
from [jszule].[dbo].[user_location] as t
where t.lon > -107.25  and t.lon < -106.1
and t.lat > 34.5 and t.lat < 35.8 and t.is_day=0
";

            var hist = new Layers.Histogram();
            hist.Color = Color.Yellow;
            hist.DataSource = new DataSources.SqlQuery()
            {
                ConnectionString = cstr,
                Command = new SqlCommand(sql)
            };

            var alpha = new Layers.Geography();
            hist.Alpha = alpha;
            alpha.FillStyle.Method = FillStyle.FillMethod.Constant;
            alpha.FillStyle.Constant = new SolidBrush(Color.FromArgb(255, Color.Black));
            alpha.LineStyle.Method = LineStyle.DrawMethod.Constant;
            alpha.LineStyle.Constant = new Pen(Color.FromArgb(255, Color.Black));

            alpha.DataSource = geo.DataSource;

            


            var bg = new Layers.Background();
            bg.Color = Color.FromArgb(255, 21, 23, 62);
            bg.DataSource = DataSource.Null;

#if false
            // Graph edges

            var graph = new Layers.Geography();
            graph.FillStyle.Method = FillStyle.FillMethod.Empty;
            graph.LineStyle.Method = LineStyle.DrawMethod.Constant;
            graph.LineStyle.Constant = new Pen(Color.FromArgb(20, Color.White));

            var edges = new DataSources.Geography();
            graph.DataSource = edges;

            sql = @"
SELECT TOP 100000 t1.lon, t1.lat, t2.lon, t2.lat
FROM Twitter..tweet t1
INNER JOIN Twitter..tweet t2 ON t2.tweet_id = t1.in_reply_to_tweet_id
WHERE
	t1.run_id = 1004 AND t2.run_id = 1004 AND
	t1.in_reply_to_tweet_id IS NOT NULL
	AND t1.lon IS NOT NULL
	AND t2.lon IS NOT NULL
    AND t1.Lon BETWEEN -125 AND -64 AND t1.Lat BETWEEN 24 AND 52
    AND t2.Lon BETWEEN -125 AND -64 AND t2.Lat BETWEEN 24 AND 52
    AND (t1.Lon <> t2.Lon OR t1.Lat <> t2.Lat)
";
            /*
            using (var cn = new SqlConnection(cstr))
            {
                cn.Open();

                using (var cmd = new SqlCommand(sql, cn))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var str = String.Format("LINESTRING({0} {1}, {2} {3})", dr.GetDouble(0), dr.GetDouble(1), dr.GetDouble(2), dr.GetDouble(3));
                            var edge = SqlGeography.STLineFromText(new SqlChars(str), 4326);
                            edges.Geographies.Add(edge);
                        }
                    }
                }
            }*/
#endif

            var grid = new Layers.Grid();
            grid.DataSource = DataSource.Null;
            grid.LineStyle.Method = LineStyle.DrawMethod.Constant;
            grid.LineStyle.Constant = new Pen(Color.FromArgb(128, Color.Black));

            var point = new Layers.Geography();
            var pg = new DataSources.Geography();
            pg.Geographies.Add(SqlGeography.Point(90, 0, 4326));
            pg.Geographies.Add(SqlGeography.Point(0, 0, 4326));
            pg.Geographies.Add(SqlGeography.Point(-90, 0, 4326));
            point.DataSource = pg;
            point.LineStyle.Method = LineStyle.DrawMethod.Constant;
            point.LineStyle.Constant = new Pen(Brushes.Red, 3);

            //map.Layers.Add(point);
            map.Layers.Add(grid);
            //map.Layers.Add(graph);
            map.Layers.Add(hist);
            map.Layers.Add(geo);
            map.Layers.Add(bg);

            var bmp = new Bitmap(1280, 1280);
            map.Render(bmp, new Rectangle(0, 0, 1280, 1280));

            bmp.Save("test.png", ImageFormat.Png);
        }
    }
}
