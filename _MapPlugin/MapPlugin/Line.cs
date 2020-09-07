//
//          Copyright Seth Hendrick 2020.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Collections.Generic;

namespace MapPlugin
{
    public class Line
    {
        // ---------------- Fields ----------------

        private readonly List<GpsCoordinate> coordinates;

        // ---------------- Constructor ----------------

        public Line()
        {
            this.coordinates = new List<GpsCoordinate>();
            this.Coordinates = this.coordinates.AsReadOnly();

            this.Id = NextId++;
        }

        static Line()
        {
            NextId = 0;
        }

        // ---------------- Properties ----------------

        public string Name { get; private set; }

        public IReadOnlyList<GpsCoordinate> Coordinates { get; private set; }

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
                        $"{nameof( Line )} at {context} does not contain key {nameKey}"
                    );
                }
                else if( string.IsNullOrWhiteSpace( dict[nameKey].ToString() ) )
                {
                    throw new PageConfigurationException(
                        $"{nameof( Line )} at {context} does not have a {nameKey}"
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
                        $"{nameof( Line )} at {context} does not contain key {coordKey}"
                    );
                }

                IList<object> coordsList = dict[coordKey] as IList<object>;
                foreach( object obj in coordsList )
                {
                    if( ( obj is IList<string> ) == false )
                    {
                        throw new PageConfigurationException(
                            $"{nameof( Line )} at {context} does not have a list at key {coordKey}"
                        );
                    }
                }

                foreach( IList<string> coordList in coordsList ) // C# will cast this for us for better or for worse.
                {
                    GpsCoordinate coord = new GpsCoordinate();
                    coord.Deserialize( $"{context}'s {coordKey}", coordList );
                    this.coordinates.Add( coord );
                }
            }
        }
    }
}
