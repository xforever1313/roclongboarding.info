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

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Renderers.Html.Inlines;
using Markdig.Syntax.Inlines;

namespace MapPlugin
{
    public sealed class RocLongMarkdownExtension : IMarkdownExtension
    {
        // ---------------- Fields ----------------

        private static readonly Regex mapIdRegex = new Regex(
            @"^#map_(?<docid>\w+)",
            RegexOptions.ExplicitCapture | RegexOptions.Compiled
        );

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            var htmlRender = renderer as HtmlRenderer;
            if (htmlRender is null)
            {
                return;
            }

            var linkRender = renderer.ObjectRenderers.FindExact<LinkInlineRenderer>();
            if (linkRender is not null)
            {
                linkRender.TryWriters.Remove(TryLinkInlineRenderer);
                linkRender.TryWriters.Add(TryLinkInlineRenderer);
            }

            var autoLinkRender = renderer.ObjectRenderers.Find<AutolinkInlineRenderer>();
            if (autoLinkRender is not null)
            {
                autoLinkRender.TryWriters.Remove(TryAutoLinkInlineRenderer);
                autoLinkRender.TryWriters.Add(TryAutoLinkInlineRenderer);
            }
        }

        private bool TryLinkInlineRenderer(HtmlRenderer renderer, LinkInline linkInline)
        {
            if (IsMapIdNode(linkInline.Url, out string docId))
            {
                linkInline.SetAttributes(
                    new HtmlAttributes()
                    {
                        Properties = new List<KeyValuePair<string, string?>>()
                        {
                            new KeyValuePair<string, string>( "href", "#map" ),
                            new KeyValuePair<string, string>( "id", docId )
                        }
                    }
                );

                linkInline.Url = "#map";
            }

            return false;
        }

        private bool TryAutoLinkInlineRenderer(HtmlRenderer renderer, AutolinkInline linkInline)
        {
            if (IsMapIdNode(linkInline.Url, out string docId))
            {
                linkInline.SetAttributes(
                    new HtmlAttributes()
                    {
                        Properties = new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>( "id", docId )
                        }
                    }
                );

                linkInline.Url = "#map";
            }

            return false;
        }

        private bool IsMapIdNode( string url, out string docId )
        {
            docId = null;

            if( url is null )
            {
                return false;
            }
            
            // instruct the formatter to process all nested nodes automatically
            Match match = mapIdRegex.Match(url);
            if (
                match.Success &&
                (string.IsNullOrWhiteSpace(match.Groups["docid"].Value) == false)
            )
            {
                docId = match.Groups["docid"].Value;
                return true;
            }

            return false;
        }
    }
}