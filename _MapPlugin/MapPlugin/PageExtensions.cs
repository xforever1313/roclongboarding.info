//
//          Copyright Seth Hendrick 2020.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Collections.Generic;
using Pretzel.Logic.Templating.Context;

namespace MapPlugin
{
    public static class PublicPageExtensions
    {
        public static bool ShouldDisplayMap( this Page page )
        {
            const string keyName = "display_map";

            if( page.Bag.ContainsKey( keyName ) == false )
            {
                return false;
            }

            if( ( page.Bag[keyName] is bool ) == false )
            {
                throw new PageConfigurationException(
                    $"Page at {page.File} does not have a {nameof( Boolean )} set for {keyName}"
                );
            }

            return (bool)page.Bag[keyName];
        }

        public static bool ShouldDisplayElevation( this Page page )
        {
            const string keyName = "display_elevation";

            if( page.Bag.ContainsKey( keyName ) == false )
            {
                return false;
            }

            if( ( page.Bag[keyName] is bool ) == false )
            {
                throw new PageConfigurationException(
                    $"Page at {page.File} does not have a {nameof( Boolean )} set for {keyName}"
                );
            }

            return (bool)page.Bag[keyName];
        }

        public static string GetAuthor( this Page page )
        {
            if( page.Bag.ContainsKey( "author" ) == false )
            {
                return null;
            }

            return page.Bag["author"].ToString();
        }
    }

    internal static class InternalPageExtensions
    {
        public static IList<string> GetCenterPointList( this Page page )
        {
            const string keyName = "center_point";

            if( page.Bag.ContainsKey( keyName ) == false )
            {
                return null;
            }

            IList<string> centerPoint = page.Bag[keyName] as IList<string>;

            if( centerPoint == null )
            {
                throw new PageConfigurationException(
                    $"Page at {page.File} does not have a list for {keyName}"
                );
            }

            return centerPoint;
        }

        public static IList<IDictionary<string, object>> GetPointsOfInterest( this Page page )
        {
            return GetList( page, "points_of_interest" );
        }

        public static IList<IDictionary<string, object>> GetHazards( this Page page )
        {
            return GetList( page, "hazards" );
        }

        public static IList<IDictionary<string, object>> GetLines( this Page page, Rating type )
        {
            string keyName;
            if( type == Rating.Cool )
            {
                keyName = "cool_lines";
            }
            else if( type == Rating.Meh )
            {
                keyName = "meh_lines";
            }
            else if( type == Rating.Lame )
            {
                keyName = "lame_lines";
            }
            else
            {
                throw new ArgumentException(
                    $"Unknown {nameof( Rating )}: {type}",
                    nameof( type )
                );
            }

            return GetList( page, keyName );
        }

        public static IList<IDictionary<string, object>> GetPolys( this Page page, Rating type )
        {
            string keyName;
            if( type == Rating.Cool )
            {
                keyName = "cool_polys";
            }
            else if( type == Rating.Meh )
            {
                keyName = "meh_polys";
            }
            else if( type == Rating.Lame )
            {
                keyName = "lame_polys";
            }
            else
            {
                throw new ArgumentException(
                    $"Unknown {nameof( Rating )}: {type}",
                    nameof( type )
                );
            }

            return GetList( page, keyName );
        }

        /// <returns>Null if the dictionary does not exist.</returns>
        private static IList<IDictionary<string, object>> GetList( this Page page, string keyName )
        {
            if( page.Bag.ContainsKey( keyName ) == false )
            {
                return null;
            }

            IList<object> list = page.Bag[keyName] as IList<object>;
            if( list == null )
            {
                throw new PageConfigurationException(
                    $"Page at {page.File} does not have a list for {keyName}"
                );
            }

            List<IDictionary<string, object>> dictList = new List<IDictionary<string, object>>();
            foreach( object l in list )
            {
                IDictionary<string, object> dict = l as IDictionary<string, object>;
                if( dict == null )
                {
                    throw new PageConfigurationException(
                        $"Page at {page.File} does not have a dictionary for {keyName}"
                    );
                }
                dictList.Add( dict );
            }

            return dictList;
        }
    }
}
