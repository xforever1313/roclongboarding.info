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
using System.Collections.ObjectModel;
using System.Composition;
using Pretzel.Logic.Extensibility;
using Pretzel.Logic.Templating.Context;

namespace MapPlugin
{
    [Export( typeof( IBeforeProcessingTransform ) )]
    public class MapInfoCollection : IBeforeProcessingTransform
    {
        // ---------------- Fields ----------------

        private Dictionary<string, MapInfo> mapInfo;

        // ---------------- Constructor ----------------

        public MapInfoCollection()
        {
            this.mapInfo = new Dictionary<string, MapInfo>();
            this.PageMapInfo = new ReadOnlyDictionary<string, MapInfo>( this.mapInfo );
        }

        static MapInfoCollection()
        {
        }

        // ---------------- Properties ----------------
        
        public static MapInfoCollection Instance { get; private set; }

        /// <summary>
        /// Dictionary that contains the map info for each page.
        /// The key is <see cref="Page.Id"/>.
        /// </summary>
        public IReadOnlyDictionary<string, MapInfo> PageMapInfo { get; private set; }

        // ---------------- Functions ----------------

        public void Transform( SiteContext context )
        {
            List<Exception> exceptions = new List<Exception>();
            this.mapInfo.Clear();

            foreach( Page page in context.Posts )
            {
                // Only posts contain map information.
                if( page.Bag.ContainsKey( "layout" ) && "location".Equals( page.Layout, StringComparison.InvariantCultureIgnoreCase ) )
                { 
                    try
                    {
                        if( page.ShouldDisplayMap() )
                        {
                            MapInfo info = new MapInfo();
                            info.Deserialize( page );
                            this.mapInfo.Add( page.Id, info );
                        }
                    }
                    catch( Exception e )
                    {
                        exceptions.Add( e );
                    }
                }
            }

            if( exceptions.Count != 0 )
            {
                throw new AggregateException( "Error when collecting map data.", exceptions );
            }

            Instance = this;
        }
    }
}
