//
// Rochester Longboarding Website Plugin - Extensions to Pretzel.
// Copyright (C) 2022 Seth Hendrick
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
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
