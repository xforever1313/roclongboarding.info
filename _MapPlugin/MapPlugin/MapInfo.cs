//
//          Copyright Seth Hendrick 2020.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Collections.Generic;
using Pretzel.Logic.Templating.Context;

namespace MapPlugin
{
    public class MapInfo
    {
        // ---------------- Fields ----------------

        private readonly List<Hazard> hazards;
        private readonly List<PointOfInterest> pois;

        private List<Polygon> coolPolygons;
        private List<Polygon> mehPolygons;
        private List<Polygon> lamePolygons;

        private List<Line> coolLines;
        private List<Line> mehLines;
        private List<Line> lameLines;

        // ---------------- Constructor ----------------

        public MapInfo()
        {
            this.hazards = new List<Hazard>();
            this.Hazards = this.hazards.AsReadOnly();

            this.pois = new List<PointOfInterest>();
            this.PointsOfInterest = this.pois.AsReadOnly();

            this.coolPolygons = new List<Polygon>();
            this.CoolPolygons = this.coolPolygons.AsReadOnly();

            this.mehPolygons = new List<Polygon>();
            this.MehPolygons = this.mehPolygons.AsReadOnly();

            this.lamePolygons = new List<Polygon>();
            this.LamePolygons = this.lamePolygons.AsReadOnly();

            this.coolLines = new List<Line>();
            this.CoolLines = this.coolLines.AsReadOnly();

            this.mehLines = new List<Line>();
            this.MehLines = this.mehLines.AsReadOnly();

            this.lameLines = new List<Line>();
            this.LameLines = this.lameLines.AsReadOnly();
        }

        // ---------------- Properties ----------------

        public Rating OverallRating { get; private set; }

        public GpsCoordinate CenterPoint { get; private set; }

        public IReadOnlyList<Hazard> Hazards { get; private set; }

        public IReadOnlyList<PointOfInterest> PointsOfInterest { get; private set; }

        public IReadOnlyList<Polygon> CoolPolygons { get; private set; }
        public IReadOnlyList<Polygon> MehPolygons { get; private set; }
        public IReadOnlyList<Polygon> LamePolygons { get; private set; }

        public IReadOnlyList<Line> CoolLines { get; private set; }
        public IReadOnlyList<Line> MehLines { get; private set; }
        public IReadOnlyList<Line> LameLines { get; private set; }

        // ---------------- Functions ----------------

        public void Deserialize( Page context )
        {
        }
    }
}
