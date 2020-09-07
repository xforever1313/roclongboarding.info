//
//          Copyright Seth Hendrick 2020.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Collections.Generic;

namespace MapPlugin
{
    public struct GpsCoordinate
    {
        // ---------------- Properties ----------------

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        // ---------------- Functions ----------------

        internal void Deserialize( string context, IList<string> coords )
        {
            if( coords == null )
            {
                throw new ArgumentNullException(
                    context,
                    nameof( coords )
                );
            }

            if( coords.Count != 2 )
            {
                throw new PageConfigurationException(
                    $"{nameof( GpsCoordinate )} list at {context} is not 2 elements long"
                );
            }

            if( double.TryParse( coords[0], out double lat ) == false )
            {
                this.Latitude = lat;
            }
            else
            {
                throw new PageConfigurationException(
                    $"{nameof( Latitude )} at {context} is not a double. Got: {coords[0]}"
                );
            }

            if( double.TryParse( coords[1], out double lon ) == false )
            {
                this.Longitude = lon;
            }
            else
            {
                throw new PageConfigurationException(
                    $"{nameof( Longitude )} at {context} is not a double. Got: {coords[1]}"
                );
            }
        }
    }
}
