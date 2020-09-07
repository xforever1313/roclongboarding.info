//
//          Copyright Seth Hendrick 2020.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Collections.Generic;

namespace MapPlugin
{
    public struct GpsCoordinate
    {
        // ---------------- Properties ----------------

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        // ---------------- Functions ----------------

        internal void Deserialize( IList<string> coords )
        {
        }
    }
}
