//
//          Copyright Seth Hendrick 2020.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Collections.Generic;

namespace MapPlugin
{
    public class Polygon
    {
        // ---------------- Fields ----------------

        private readonly List<GpsCoordinate> coordinates;

        // ---------------- Constructor ----------------

        public Polygon()
        {
            this.coordinates = new List<GpsCoordinate>();
            this.Coordinates = this.coordinates.AsReadOnly();
        }

        // ---------------- Properties ----------------

        public IReadOnlyList<GpsCoordinate> Coordinates { get; private set; }

        // ---------------- Functions ----------------

        internal void Deserialize( object bag )
        {
        }
    }
}
