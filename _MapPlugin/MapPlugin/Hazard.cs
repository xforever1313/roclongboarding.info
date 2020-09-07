//
//          Copyright Seth Hendrick 2020.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

namespace MapPlugin
{
    public class Hazard
    {
        // ---------------- Constructor ----------------

        public Hazard()
        {
            this.Name = string.Empty;
        }

        // ---------------- Properties ----------------

        public string Name { get; private set; }

        public GpsCoordinate Coordinate { get; private set; }

        // ---------------- Functions ----------------

        internal void Deserialize( object bag )
        {
        }
    }
}
