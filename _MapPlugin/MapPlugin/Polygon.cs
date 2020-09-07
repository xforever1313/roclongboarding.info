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

        public string Name { get; private set; }

        public IReadOnlyList<GpsCoordinate> Coordinates { get; private set; }

        // ---------------- Functions ----------------

        internal void Deserialize( string context, IDictionary<string, object> dict )
        {
            {
                const string nameKey = "name";

                if( dict.ContainsKey( nameKey ) == false )
                {
                    throw new PageConfigurationException(
                        $"{nameof( Polygon )} at {context} does not contain key {nameKey}"
                    );
                }
                else if( string.IsNullOrWhiteSpace( dict[nameKey].ToString() ) )
                {
                    throw new PageConfigurationException(
                        $"{nameof( Polygon )} at {context} does not have a {nameKey}"
                    );
                }
                else
                {
                    this.Name = dict[nameKey].ToString();
                }
            }

            {
                const string coordKey = "coords";

                if( dict.ContainsKey( coordKey ) == false )
                {
                    throw new PageConfigurationException(
                        $"{nameof( Polygon )} at {context} does not contain key {coordKey}"
                    );
                }

                IList<IList<string>> coords = dict[coordKey] as IList<IList<string>>;

                foreach( IList<string> coordList in coords )
                {
                    GpsCoordinate coord = new GpsCoordinate();
                    coord.Deserialize( $"{context}'s {coordKey}", coordList );
                    this.coordinates.Add( coord );
                }
            }
        }
    }
}
