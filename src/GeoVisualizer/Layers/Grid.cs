using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Types;

namespace Elte.GeoVisualizer.Lib.Layers
{
    public class Grid : Geography
    {
        private double majorStep;

        public double MajorStep
        {
            get { return majorStep; }
            set { majorStep = value; }
        }

        public Grid()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.majorStep = 1;    // degrees
        }

        public override void OnBeginRender(RenderingContext context)
        {
            base.OnBeginRender(context);

            var sb = new SqlGeographyBuilder();
            sb.SetSrid(4326);
            sb.BeginGeography(OpenGisGeographyType.MultiLineString);

            double lat = context.Projection.GeoMin.Lat;
            lat = Math.Floor(lat / majorStep) * majorStep;

            for (; lat <= context.Projection.GeoMax.Lat; lat += majorStep)
            {
                AddLatitudeLine(sb, context.Projection.GeoMin.Lon, context.Projection.GeoMax.Lon, lat);
            }

            // Longitudes are great circles
            double lon = context.Projection.GeoMin.Lon;
            lon = Math.Floor(lon / majorStep) * majorStep;

            for (; lon <= context.Projection.GeoMax.Lon; lon += majorStep)
            {
                AddLongitudeLine(sb, lon, context.Projection.GeoMin.Lat, context.Projection.GeoMax.Lat);
            }

            sb.EndGeography();
            RenderGeography(context, sb.ConstructedGeography);
        }

        public override void OnRender(RenderingContext context, object[] values)
        {
            // Do nothing here   
        }

        private void AddLatitudeLine(SqlGeographyBuilder sb, double lonmin, double lonmax, double lat)
        {
            // TODO: calculate number of steps from tolerance

            sb.BeginGeography(OpenGisGeographyType.LineString);
            sb.BeginFigure(lat, lonmin);

            for (int i = 0; i < 100; i++)
            {
                sb.AddLine(lat, lonmin + (i + 1) * (lonmax - lonmin) / 100);
            }

            sb.EndFigure();
            sb.EndGeography();
        }

        private void AddLongitudeLine(SqlGeographyBuilder sb, double lon, double latmin, double latmax)
        {
            sb.BeginGeography(OpenGisGeographyType.LineString);
            sb.BeginFigure(latmin, lon);
            sb.AddLine(latmax, lon);
            sb.EndFigure();
            sb.EndGeography();
        }
    }
}
