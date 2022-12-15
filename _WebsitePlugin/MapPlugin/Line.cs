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
using System.IO;
using System.Linq;
using System.Xml.Linq;

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
            this.GpxFile = null;

            this.Id = NextId++;
        }

        static Line()
        {
            NextId = 0;
        }

        // ---------------- Properties ----------------

        public string Name { get; private set; }

        public IReadOnlyList<GpsCoordinate> Coordinates { get; private set; }

        public string GpxFile { get; private set; }

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

                // If we are a string, it means we want to parse a GPX file.
                // If we are not a string, it means that the coordinates are hard-coded.
                if( dict[coordKey] is string gpxFile )
                {
                    DeserializeGpxData( context, gpxFile );
                    this.GpxFile = gpxFile;
                }
                else
                {
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
                        this.AddCoordIfNotExist( coord );
                    }
                }
            }
        }

        internal void DeserializeGpxData( string context, string inputFile )
        {
            if( File.Exists( inputFile ) == false )
            {
                throw new PageConfigurationException(
                    $"{nameof( Line )} at {context} can not find GPX file at {inputFile}"
                );
            }

            XDocument doc = XDocument.Load( inputFile );
            XElement root = doc.Root;

            if( "gpx".Equals( root.Name.LocalName ) == false )
            {
                throw new PageConfigurationException(
                    $"{nameof( Line )} at {context} does not have 'gpx' in the XML root in {inputFile}"
                );
            }

            XElement trkElement = doc.Root.Elements().FirstOrDefault( e => "trk".Equals( e.Name.LocalName ) );
            if( trkElement == null )
            {
                throw new PageConfigurationException(
                    $"{nameof( Line )} at {context} can not find 'trk' element in file at {inputFile}"
                );
            }

            List<XElement> trkPts = new List<XElement>();
            foreach( XElement trkChild in trkElement.Elements() )
            {
                if( "trkseg".Equals( trkChild.Name.LocalName ) )
                {
                    foreach( XElement trkseqChild in trkChild.Elements() )
                    {
                        if( "trkpt".Equals( trkseqChild.Name.LocalName ) )
                        {
                            trkPts.Add( trkseqChild );
                        }
                    }
                }
            }

            int CompareNodes( XElement left, XElement right )
            {
                try
                {
                    DateTime leftDate = DateTime.Parse( left.Elements().Where( e => "time".Equals( e.Name.LocalName ) ).FirstOrDefault()?.Value );
                    DateTime rightDate = DateTime.Parse( right.Elements().Where( e => "time".Equals( e.Name.LocalName ) ).FirstOrDefault()?.Value );

                    return leftDate.CompareTo( rightDate );
                }
                catch( Exception e )
                {
                    throw new PageConfigurationException(
                        $"{nameof( Line )} at {context} could not parse 'time' element in {inputFile} for reason {e.Message}",
                        e
                    );
                }
            }

            trkPts.Sort( CompareNodes );

            foreach( XElement trkPt in trkPts )
            {
                GpsCoordinate coord = new GpsCoordinate();

                {
                    XAttribute latAttribute = trkPt.Attribute( "lat" );
                    if( latAttribute == null )
                    {
                        throw new PageConfigurationException(
                            $"{nameof( Line )} at {context} could find 'lat' element in {inputFile}"
                        );
                    }
                    else
                    {
                        if( double.TryParse( latAttribute.Value, out double lat ) )
                        {
                            coord.Latitude = lat;
                        }
                        else
                        {
                            throw new PageConfigurationException(
                                $"{nameof( Line )} at {context} could not parse 'lat' element in {inputFile}, got {latAttribute.Value ?? "null"}"
                            );
                        }
                    }
                }

                {
                    XAttribute lonAttribute = trkPt.Attribute( "lon" );
                    if( lonAttribute == null )
                    {
                        throw new PageConfigurationException(
                            $"{nameof( Line )} at {context} could find 'lon' element in {inputFile}"
                        );
                    }
                    else
                    {
                        if( double.TryParse( lonAttribute.Value, out double lon ) )
                        {
                            coord.Longitude = lon;
                        }
                        else
                        {
                            throw new PageConfigurationException(
                                $"{nameof( Line )} at {context} could not parse 'lon' element in {inputFile}, got {lonAttribute.Value ?? "null"}"
                            );
                        }
                    }
                }

                {
                    XElement elevElement = trkPt.Elements().Where( e => "ele".Equals( e.Name.LocalName ) ).FirstOrDefault();
                    if( elevElement != null )
                    {
                        if( double.TryParse( elevElement.Value, out double elevation ) )
                        {
                            coord.Elevation = elevation;
                        }
                        else
                        {
                            throw new PageConfigurationException(
                                $"{nameof( Line )} at {context} could not parse 'ele' element in {inputFile}, got {elevElement.Value ?? "null"}"
                            );
                        }
                    }
                }

                this.AddCoordIfNotExist( coord );
            }
        }

        /// <summary>
        /// Only adds a coordinate if it the lat/long does not exist yet.
        /// This is done to reduce the number of points on the map.
        /// </summary>
        private void AddCoordIfNotExist( GpsCoordinate coord )
        {
            if( this.Coordinates.Any( c => c.EqualsIgnoreElevation( coord ) ) == false )
            {
                this.coordinates.Add( coord );
            }
        }
    }
}
