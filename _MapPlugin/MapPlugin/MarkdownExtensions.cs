//
//          Copyright Seth Hendrick 2020.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System.Text.RegularExpressions;
using CommonMark;
using CommonMark.Syntax;

namespace MapPlugin
{
    /// <summary>
    /// Inspired from: https://github.com/Knagis/CommonMark.NET/wiki#htmlformatter
    /// </summary>
    public class CustomHtmlFormatter : CommonMark.Formatters.HtmlFormatter
    {
        // ---------------- Fields ----------------

        private static readonly Regex mapIdRegex = new Regex(
            @"#map_(?<docid>\w+)",
            RegexOptions.ExplicitCapture | RegexOptions.Compiled  
        );

        // ---------------- Constructor ----------------

        public CustomHtmlFormatter( System.IO.TextWriter target, CommonMarkSettings settings )
            : base(target, settings)
        {
        }

        // ---------------- Functions ----------------

        protected override void WriteInline( Inline inline, bool isOpening, bool isClosing, out bool ignoreChildNodes )
        {
            if (
                // verify that the inline element is one that should be modified
                ( inline.Tag == InlineTag.Link ) &&
                // verify that the formatter should output HTML and not plain text
                ( this.RenderPlainTextInlines.Peek() == false )
            )
            {
                // If our URL starts with http, its probably outgoing,
                // so add target=_blank, but also other security tags
                // see also: https://www.pixelstech.net/article/1537002042-The-danger-of-target=_blank-and-opener
                if( inline.TargetUrl.StartsWith( "http" ) )
                {
                    // instruct the formatter to process all nested nodes automatically
                    ignoreChildNodes = false;

                    // start and end of each node may be visited separately
                    if( isOpening )
                    {
                        this.Write("<a target=\"_blank\" rel=\"noopener noreferrer nofollow\" href=\"");
                        this.WriteEncodedUrl(inline.TargetUrl);
                        this.Write("\">");
                    }

                    // note that isOpening and isClosing can be true at the same time
                    if( isClosing )
                    {
                        this.Write("</a>");
                    }
                }
                else if ( inline.TargetUrl.StartsWith( "#map_" ) )
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
                            this.Write("</a>");
                        }
                    }
                    else
                    {
                        base.WriteInline( inline, isOpening, isClosing, out ignoreChildNodes );  
                    }
                }
                else
                {
                    // in all other cases the default implementation will output the correct HTML
                    base.WriteInline( inline, isOpening, isClosing, out ignoreChildNodes );   
                }
            }
            else
            {
                // in all other cases the default implementation will output the correct HTML
                base.WriteInline( inline, isOpening, isClosing, out ignoreChildNodes );
            }
        }
    }
}
