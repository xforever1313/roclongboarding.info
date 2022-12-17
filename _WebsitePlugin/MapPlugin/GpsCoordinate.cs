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

using System;
using System.Collections.Generic;
using Geodesy;

namespace MapPlugin
{
    public struct GpsCoordinate
    {
        // ---------------- Properties ----------------

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        /// <summary>
        /// The elevation of the GPS coordinate.
        /// Null for none specified.
        /// </summary>
        public double? Elevation { get; set; }

        // ---------------- Functions ----------------

        public override bool Equals( object obj )
        {
            if( obj is GpsCoordinate coordinate )
            {
                return Equals( coordinate );
            }

            return false;
        }

        public bool Equals( GpsCoordinate other )
        {
            return
                EqualsIgnoreElevation( other ) &&
                ( this.Elevation == other.Elevation );
        }

        public bool EqualsIgnoreElevation( GpsCoordinate other )
        {
            return
                ( this.Latitude == other.Latitude ) &&
                ( this.Longitude == other.Longitude );
        }

        public override int GetHashCode()
        {
            // Mutable.  Return base implementation.
            return base.GetHashCode();
        }

        internal void Deserialize( string context, IList<string> coords )
        {
            if( coords == null )
            {
                throw new ArgumentNullException(
                    context,
                    nameof( coords )
                );
            }

            if( ( coords.Count != 2 ) && ( coords.Count != 3 ) )
            {
                throw new PageConfigurationException(
                    $"{nameof( GpsCoordinate )} list at {context} is not 2 elements long"
                );
            }

            if( double.TryParse( coords[0], out double lat ) )
            {
                this.Latitude = lat;
            }
            else
            {
                throw new PageConfigurationException(
                    $"{nameof( Latitude )} at {context} is not a double. Got: {coords[0]}"
                );
            }

            if( double.TryParse( coords[1], out double lon ) )
            {
                this.Longitude = lon;
            }
            else
            {
                throw new PageConfigurationException(
                    $"{nameof( Longitude )} at {context} is not a double. Got: {coords[1]}"
                );
            }

            if( coords.Count == 3 )
            {
                if( double.TryParse( coords[2], out double elev ) )
                {
                    this.Elevation = elev;
                }
                else
                {
                    throw new PageConfigurationException(
                        $"{nameof( Elevation )} at {context} is not a double. Got: {coords[2]}"
                    );
                }
            }
            else
            {
                this.Elevation = null;
            }
        }
    }

    public static class GpsCoordinateExtensions
    {
        public static double CalculateDistanceKm( this IReadOnlyList<GpsCoordinate> coords )
        {
            double distance = 0;
            GeodeticCalculator geoCalc = new GeodeticCalculator( Ellipsoid.WGS84 );

            for( int i = 0; i < ( coords.Count - 1 ); ++i )
            {
                GlobalCoordinates startCords = new GlobalCoordinates(
                    new Angle( coords[i].Latitude ),
                    new Angle( coords[i].Longitude )
                );
                GlobalPosition startPos = new GlobalPosition( startCords );

                GlobalCoordinates endCords = new GlobalCoordinates(
                    new Angle( coords[i + 1].Latitude ),
                    new Angle( coords[i + 1].Longitude )
                );
                GlobalPosition endPos = new GlobalPosition( endCords );

                GeodeticMeasurement result = geoCalc.CalculateGeodeticMeasurement( startPos, endPos );
                distance += result.PointToPointDistance;
            }

            return distance / 1000.0;
        }
    }
}
