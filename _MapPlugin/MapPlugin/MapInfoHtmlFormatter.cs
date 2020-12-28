//
//          Copyright Seth Hendrick 2020.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Text.RegularExpressions;
using CommonMark;
using CommonMark.Syntax;
using Pretzel.SethExtensions;

namespace MapPlugin
{
    /// <summary>
    /// Inspired from: https://github.com/Knagis/CommonMark.NET/wiki#htmlformatter
    /// </summary>
    public class MapInfoHtmlFormatter : SethHtmlFormatter
    {
        // ---------------- Fields ----------------

        private static readonly Regex mapIdRegex = new Regex(
            @"#map_(?<docid>\w+)",
            RegexOptions.ExplicitCapture | RegexOptions.Compiled  
        );

        // ---------------- Constructor ----------------

        public MapInfoHtmlFormatter( System.IO.TextWriter target, CommonMarkSettings settings )
            : base( target, settings )
        {
        }

        // ---------------- Functions ----------------

        protected override void WriteInlineLinkTag( Inline inline, bool isOpening, bool isClosing, out bool ignoreChildNodes )
        {
            if( inline.TargetUrl.StartsWith( "#map_" ) )
            {
                // instruct the formatter to process all nested nodes automatically
                Match match = mapIdRegex.Match( inline.TargetUrl );
                if(
                    match.Success &&
                    ( string.IsNullOrWhiteSpace( match.Groups["docid"].Value ) == false )
                )
                {
                    ignoreChildNodes = false;

                    // start and end of each node may be visited separately
                    if( isOpening )
                    {
                        this.Write(
                            $@"<a href=""#map"" id=""{match.Groups["docid"].Value}"">"
                        );
                    }

                    // note that isOpening and isClosing can be true at the same time
                    if( isClosing )
                    {
                        this.Write( "</a>" );
                    }
                }
                else
                {
                    base.WriteInlineLinkTag( inline, isOpening, isClosing, out ignoreChildNodes );
                }
            }
            else
            {
                base.WriteInlineLinkTag( inline, isOpening, isClosing, out ignoreChildNodes );
            }
        }
    }
}
