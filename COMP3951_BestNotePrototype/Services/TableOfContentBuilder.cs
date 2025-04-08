using Markdig;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using System.Text;

namespace BestNote_3951.Services
{
    /// <summary>
    /// Table of contents builder is a service that handles the markdown rendering
    /// and html formatting such that heading are automatically added to the top of 
    /// the document.
    /// 
    /// Takes the static global markdown pipeline from App.Pipeline. Since it has 
    /// UseAutoIdentifiers we can access the id of the headings we create in the markdown
    /// doc. The markdownDocument gives allows us to check the syntax tree and collect
    /// all of the headings. We iterate over all of the headings and append them to a 
    /// stringbuilder that gets inserted as a chunk above the newmarkdown.
    /// </summary>
    public static class TableOfContentBuilder
    {
        public static string TableOfContentizer(string markdown)
        {
            MarkdownPipeline pipeline          = App.Pipeline;
            MarkdownDocument doc               = Markdown.Parse(markdown, pipeline);
            IEnumerable<HeadingBlock> headings = doc.Descendants<HeadingBlock>();
            Color color                        = (Color)Application.Current!.Resources["BorderBackground"];
            String paneColor                   = color.ToArgbHex(false);
            color                              = (Color)Application.Current!.Resources["EditorText"];
            String textColor                   = color.ToArgbHex(false);

            int[] counters    = new int[7];
            int previousLevel = 0;

            StringBuilder toc = new StringBuilder();

            toc.AppendLine(@"<style>
                nav {
                    font-family: Arial, sans-serif;
                    font-weight: bold;
                    margin-bottom: 1em;
                }
                nav ul {
                    list-style-type: none;
                    margin: 0;
                    padding: 0;
                }
                nav li {
                    margin: 0.25em 0;
                }
                nav li a {
                    text-decoration: none;
                    color: #333;
                }
                nav li a:hover {
                    text-decoration: underline;
                }
                body {");
            toc.AppendFormat("background-color: {0};", paneColor);
            toc.AppendFormat("color: {0};", textColor);
            toc.AppendLine(@"
                }
            </style>");

            toc.AppendLine("<nav>");
            toc.AppendLine("Table of Contents");
            toc.AppendLine("<ul>");

            foreach (HeadingBlock heading in headings)
            {
                int currentLevel = heading.Level;

                // at each new heading level, increment that levels counter and reset
                // for sub levels.
                counters[currentLevel]++;
                for (int i = currentLevel + 1; i < counters.Length; i++)
                {
                    counters[i] = 0;
                }

                // add ul tags if current level is deeper than prev.
                // close ul tags if if it's shallower
                if (currentLevel > previousLevel)
                {
                    for (int i = previousLevel; i < currentLevel; i++)
                    {
                        toc.AppendLine("<ul>");
                    }
                }
                else if (currentLevel < previousLevel)
                {
                    for (int i = currentLevel; i < previousLevel; i++)
                    {
                        toc.AppendLine("</ul>");
                    }
                }

                previousLevel = currentLevel;

                string? numbering = BuildNumberString(counters, currentLevel);

                string? id                       = heading.GetAttributes().Id;
                LiteralInline? headingTextInline = heading.Inline?.FirstChild as LiteralInline;
                string headingContent            = headingTextInline?.Content.ToString() ?? "Unknown";

                toc.AppendLine($"<li><a href=\"#{id}\">{numbering} {headingContent}</a></li>");
            }

            // close all the ul tags
            for (int i = previousLevel; i > 0; i--)
            {
                toc.AppendLine("</ul>");
            }

            toc.AppendLine("</nav>");

            string newMarkdown = Markdown.ToHtml(markdown, pipeline);

            string html = $@"
                <html>
                    <head><meta charset=""utf-8"" /></head>
                    <body>
                        {toc}
                        {newMarkdown}
                    </body>
                </html>";

            return html;
        }

        /// <summary>
        /// Given the counters array and the current heading level, build a string like '1.2.1'.
        /// </summary>
        private static string BuildNumberString(int[] counters, int level)
        {
            // if counters = [0,1,2,0,0,0,0] and level = 2 then we want 1.2
            List<int> parts = [];
            for (int i = 1; i <= level; i++)
            {
                if (counters[i] > 0)
                {
                    parts.Add(counters[i]);
                }
            }
            return string.Join(".", parts);
        }
    }
}
