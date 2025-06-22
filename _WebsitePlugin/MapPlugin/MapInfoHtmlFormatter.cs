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

using System.Text.RegularExpressions;
//using CommonMark;
//using CommonMark.Syntax;
using Pretzel.SethExtensions;

namespace MapPlugin
{
    #if false
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

        public MapInfoHtmlFormatter(System.IO.TextWriter target, CommonMarkSettings settings)
            : base(target, settings)
        {
        }

        // ---------------- Functions ----------------

        protected override void WriteInlineLinkTag(Inline inline, bool isOpening, bool isClosing, out bool ignoreChildNodes)
        {
            if (inline.TargetUrl.StartsWith("#map_"))
            {
                // instruct the formatter to process all nested nodes automatically
                Match match = mapIdRegex.Match(inline.TargetUrl);
                if (
                    match.Success &&
                    (string.IsNullOrWhiteSpace(match.Groups["docid"].Value) == false)
                )
                {
                    ignoreChildNodes = false;

                    // start and end of each node may be visited separately
                    if (isOpening)
                    {
                        this.Write(
                            $@"<a href=""#map"" id=""{match.Groups["docid"].Value}"">"
                        );
                    }

                    // note that isOpening and isClosing can be true at the same time
                    if (isClosing)
                    {
                        this.Write("</a>");
                    }
                }
                else
                {
                    base.WriteInlineLinkTag(inline, isOpening, isClosing, out ignoreChildNodes);
                }
            }
            else
            {
                base.WriteInlineLinkTag(inline, isOpening, isClosing, out ignoreChildNodes);
            }
        }
    }
    #endif
}
