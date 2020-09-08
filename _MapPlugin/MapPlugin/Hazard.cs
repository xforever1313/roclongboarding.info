//
//          Copyright Seth Hendrick 2020.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Collections.Generic;

namespace MapPlugin
{
    public class Hazard
    {
        // ---------------- Constructor ----------------

        public Hazard()
        {
            this.Name = string.Empty;
            this.Id = NextId++;   
        }

        static Hazard()
        {
            NextId = 0;
        }

        // ---------------- Properties ----------------

        public string Name { get; private set; }

        public GpsCoordinate Coordinate { get; private set; }

        public string DocId { get; private set; }

        public int Id { get; private set; }

        public static int NextId { get; private set; }

        // ---------------- Functions ----------------

        internal void Deserialize( string context, IDictionary<string, object> dict )
        {
            {
                const string nameKey = "name";

                if( dict.ContainsKey( nameKey ) == false )
                {
                    throw new PageConfigurationException(
                        $"{nameof( Hazard )} at {context} does not contain key {nameKey}"
                    );
                }
                else if( string.IsNullOrWhiteSpace( dict[nameKey].ToString() ) )
                {
                    throw new PageConfigurationException(
                        $"{nameof( Hazard )} at {context} does not have a {nameKey}"
                    );
                }
                else
                {
                    this.Name = dict[nameKey].ToString();
                }
            }

            {
                const string coordKey = "coord";

                if( dict.ContainsKey( coordKey ) == false )
                {
                    throw new PageConfigurationException(
                        $"{nameof( Hazard )} at {context} does not contain key {coordKey}"
                    );
                }

                IList<string> coords = dict[coordKey] as IList<string>;

                GpsCoordinate coord = new GpsCoordinate();
                coord.Deserialize( $"{context}'s {coordKey}", coords );
                this.Coordinate = coord;
            }

            {
                const string nameKey = "docid";

                if( dict.ContainsKey( nameKey ) == false )
                {
                    // Optional.
                    return;
                }
                else if( string.IsNullOrWhiteSpace( dict[nameKey].ToString() ) == false )
                {
                    this.DocId = dict[nameKey].ToString();
                }
            }
        }
    }
}
