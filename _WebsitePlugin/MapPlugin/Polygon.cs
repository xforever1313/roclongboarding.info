﻿//
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
    public class Polygon
    {
        // ---------------- Fields ----------------

        private readonly List<GpsCoordinate> coordinates;

        // ---------------- Constructor ----------------

        public Polygon()
        {
            this.coordinates = new List<GpsCoordinate>();
            this.Coordinates = this.coordinates.AsReadOnly();

            this.Id = NextId++;
        }

        static Polygon()
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

                IList<object> coordsList = dict[coordKey] as IList<object>;
                foreach( object obj in coordsList )
                {
                    if( ( obj is IList<string> ) == false )
                    {
                        throw new PageConfigurationException(
                            $"{nameof( Polygon )} at {context} does not have a list at key {coordKey}"
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
