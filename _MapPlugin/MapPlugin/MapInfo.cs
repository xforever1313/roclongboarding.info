//
//          Copyright Seth Hendrick 2020.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Collections.Generic;
using Pretzel.Logic.Templating.Context;

namespace MapPlugin
{
    public class MapInfo
    {
        // ---------------- Fields ----------------

        private readonly List<Hazard> hazards;
        private readonly List<PointOfInterest> pois;

        private readonly List<Line> coolLines;
        private readonly List<Line> mehLines;
        private readonly List<Line> lameLines;

        private readonly List<Polygon> coolPolygons;
        private readonly List<Polygon> mehPolygons;
        private readonly List<Polygon> lamePolygons;

        // ---------------- Constructor ----------------

        public MapInfo()
        {
            this.OverallRating = Rating.Unknown;

            this.hazards = new List<Hazard>();
            this.Hazards = this.hazards.AsReadOnly();

            this.pois = new List<PointOfInterest>();
            this.PointsOfInterest = this.pois.AsReadOnly();

            this.coolLines = new List<Line>();
            this.CoolLines = this.coolLines.AsReadOnly();

            this.mehLines = new List<Line>();
            this.MehLines = this.mehLines.AsReadOnly();

            this.lameLines = new List<Line>();
            this.LameLines = this.lameLines.AsReadOnly();

            this.coolPolygons = new List<Polygon>();
            this.CoolPolygons = this.coolPolygons.AsReadOnly();

            this.mehPolygons = new List<Polygon>();
            this.MehPolygons = this.mehPolygons.AsReadOnly();

            this.lamePolygons = new List<Polygon>();
            this.LamePolygons = this.lamePolygons.AsReadOnly();

            this.Id = NextId++;
        }

        static MapInfo()
        {
            NextId = 0;
        }

        // ---------------- Properties ----------------

        public Rating OverallRating { get; private set; }

        public GpsCoordinate CenterPoint { get; private set; }

        public IReadOnlyList<PointOfInterest> PointsOfInterest { get; private set; }

        public IReadOnlyList<Hazard> Hazards { get; private set; }

        public IReadOnlyList<Line> CoolLines { get; private set; }
        public IReadOnlyList<Line> MehLines { get; private set; }
        public IReadOnlyList<Line> LameLines { get; private set; }

        public IReadOnlyList<Polygon> CoolPolygons { get; private set; }
        public IReadOnlyList<Polygon> MehPolygons { get; private set; }
        public IReadOnlyList<Polygon> LamePolygons { get; private set; }

        public int Id { get; private set; }

        public static int NextId { get; private set; }

        // ---------------- Functions ----------------

        public void Deserialize( Page context )
        {
            DetermineOveralRating( context );
            DetermineCenterPoint( context );
            DeterminePointsOfInterest( context );
            DetermineHazards( context );

            DetermineLines(
                context,
                 Rating.Cool,
                l => { this.coolLines.Add( l ); }
            );

            DetermineLines(
                context,
                 Rating.Meh,
                l => { this.mehLines.Add( l ); }
            );

            DetermineLines(
                context,
                 Rating.Lame,
                l => { this.lameLines.Add( l ); }
            );

            DeterminePolygons(
                context,
                 Rating.Cool,
                l => { this.coolPolygons.Add( l ); }
            );

            DeterminePolygons(
                context,
                 Rating.Meh,
                l => { this.mehPolygons.Add( l ); }
            );

            DeterminePolygons(
                context,
                 Rating.Lame,
                l => { this.lamePolygons.Add( l ); }
            );
        }

        private void DetermineOveralRating( Page context )
        {
            foreach( string category in context.Categories )
            {
                if( "Cool Places".Equals( category, StringComparison.InvariantCultureIgnoreCase ) )
                {
                    this.OverallRating = Rating.Cool;
                    break;
                }
                else if( "Meh Places".Equals( category, StringComparison.InvariantCultureIgnoreCase ) )
                {
                    this.OverallRating = Rating.Meh;
                }
                else if( "Lame Places".Equals( category, StringComparison.InvariantCultureIgnoreCase ) )
                {
                    this.OverallRating = Rating.Lame;
                }
            }

            if( this.OverallRating == Rating.Unknown )
            {
                throw new PageConfigurationException(
                    $"Page at {context.File} does not contain a category, at least one is needed to determine the rating."
                );
            }
        }

        private void DetermineCenterPoint( Page context )
        {
            GpsCoordinate centerPoint = new GpsCoordinate();
            centerPoint.Deserialize( context.File + " center_point", context.GetCenterPointList() );
            this.CenterPoint = centerPoint;
        }

        private void DeterminePointsOfInterest( Page context )
        {
            IList<IDictionary<string, object>> pois = context.GetPointsOfInterest();
            if( pois == null )
            {
                return;
            }

            foreach( IDictionary<string, object> poi in pois )
            {
                PointOfInterest point = new PointOfInterest();
                point.Deserialize( $"{context.File} {nameof( PointOfInterest )}", poi );
                this.pois.Add( point );
            }
        }

        private void DetermineHazards( Page context )
        {
            IList<IDictionary<string, object>> hazards = context.GetHazards();
            if( hazards == null )
            {
                return;
            }

            foreach( IDictionary<string, object> haz in hazards )
            {
                Hazard hazard = new Hazard();
                hazard.Deserialize( $"{context.File} {nameof( Hazard )}", haz );
                this.hazards.Add( hazard );
            }
        }

        private void DetermineLines( Page context, Rating rating, Action<Line> addAction )
        {
            IList<IDictionary<string, object>> lines = context.GetLines( rating );
            if( lines == null )
            {
                return;
            }

            foreach( IDictionary<string, object> l in lines )
            {
                Line line = new Line();
                line.Deserialize( $"{context.File} {rating}", l );
                addAction( line );
            }
        }

        private void DeterminePolygons( Page context, Rating rating, Action<Polygon> addAction )
        {
            IList<IDictionary<string, object>> polys = context.GetLines( rating );
            if( polys == null )
            {
                return;
            }

            foreach( IDictionary<string, object> p in polys )
            {
                Polygon line = new Polygon();
                line.Deserialize( $"{context.File} {rating}", p );
                addAction( line );
            }
        }
    }
}
