//
//          Copyright Seth Hendrick 2020.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
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

        // ---------------- Properties ----------------

        /// <summary>
        /// Dictionary that contains the map info for each page.
        /// The key is <see cref="Page.Id"/>.
        /// </summary>
        public IReadOnlyDictionary<string, MapInfo> PageMapInfo { get; private set; }

        // ---------------- Functions ----------------

        public void Transform( SiteContext context )
        {
            List<Exception> exceptions = new List<Exception>();

            foreach( Page page in context.Pages )
            {
                // Only posts contain map information.
                if( "location".Equals( page.Layout, StringComparison.InvariantCultureIgnoreCase ) )
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
        }
    }
}
