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
        const string cstr = "Data Source=future1;Initial Catalog=Gadm;Integrated Security=true;Type System Version=SQL Server 2012;";

        static void Main(string[] args)
        {
            string sql;

            System.Threading.Thread.CurrentThread.CurrentCulture =
                System.Globalization.CultureInfo.InvariantCulture;

            // Create map and define projection
            var map = new Map();
            map.Projection = new Projections.Stereographic()
            {
                //MapScale = new MapPoint(3000, 3000),     // USA
                //GeoOrigin = new GeoPoint(-96, 40),       // USA
                MapScale = new MapPoint(500, 500),
                GeoOrigin = new GeoPoint(0, 0),
                MapMin = new MapPoint(0, 0),
                MapMax = new MapPoint(1280, 1280),
                MapOffset = new MapPoint(640, 640),
            };

            // Create a solid color background layer
            var bg = new Layers.Background();
            bg.Color = Color.Black;
            bg.DataSource = DataSource.Null;

            // Create a geography layer that takes the maps from SQL
            sql = "SELECT geom.Reduce(1000) FROM Gadm..Region WHERE AdmLevel = 0 -- AND ISO = 'USA'";
            var geo = new Layers.Geography();
            geo.FillStyle.Method = FillStyle.FillMethod.Empty;
            geo.FillStyle.Constant = new SolidBrush(Color.FromArgb(255, 21, 23, 62));
            geo.LineStyle.Method = LineStyle.DrawMethod.Constant;
            geo.LineStyle.Constant = new Pen(Color.FromArgb(255, 21, 23, 62));
            geo.DataSource = new DataSources.SqlQuery()
                {
                    ConnectionString = cstr,
                    Command = new SqlCommand(sql)
                };

#if false
            // Create a histogram layer that takes point distribution from SQL
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

            // Create an alpha layer for the histogram layers
            // It's purpose is to hide those parts of the histogram that
            // are outside region boundaries (in the see, for example)
            var alpha = new Layers.Geography();
            alpha.FillStyle.Method = FillStyle.FillMethod.Constant;
            alpha.FillStyle.Constant = new SolidBrush(Color.FromArgb(255, Color.Black));
            alpha.LineStyle.Method = LineStyle.DrawMethod.Constant;
            alpha.LineStyle.Constant = new Pen(Color.FromArgb(255, Color.Black));
            alpha.DataSource = geo.DataSource;
            hist.Alpha = alpha;

            // Create a grid layer that draws a lon and lat grid
            var grid = new Layers.Grid();
            grid.DataSource = DataSource.Null;
            grid.LineStyle.Method = LineStyle.DrawMethod.Constant;
            grid.LineStyle.Constant = new Pen(Color.FromArgb(128, Color.Black));
#endif

            // Create a graph layer
            sql = @"select TOP 1000000 c_lon, c_lat, f_lon, f_lat, dist from jszule.dbo.follower_2dir_coord";
            var graph = new Layers.Graph();
            graph.LineStyle.Method = LineStyle.DrawMethod.Constant;
            graph.LineStyle.Constant = new Pen(Color.FromArgb(1, 255, 0, 0));
            graph.DataSource = new DataSources.SqlQuery()
            {
                ConnectionString = cstr,
                Command = new SqlCommand(sql),
            };

            // Add layers to the map, from bottom to up
            map.Layers.Add(graph);
            //map.Layers.Add(grid);
            //map.Layers.Add(hist);
            map.Layers.Add(geo);
            map.Layers.Add(bg);

            // Render the map to a bitmap
            var bmp = new Bitmap(1280, 1280);
            map.Render(bmp, new Rectangle(0, 0, 1280, 1280));

            // Save bitmap
            bmp.Save("test.png", ImageFormat.Png);
        }
    }
}
