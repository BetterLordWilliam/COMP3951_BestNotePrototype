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

            StringBuilder toc = new StringBuilder();
            toc.AppendLine("<nav>");
            toc.AppendLine("<ul>");

            foreach (HeadingBlock heading in headings)
            {
                string? id = heading.GetAttributes().Id;

                // literal inline is for normal text. right now headings can only be normal text
                // but should be able to handle different types of headings like heading that are 
                // pdf links or bolded/stylized but this works for now.
                LiteralInline? headingTextInline = heading.Inline?.FirstChild as LiteralInline;
                string headingContent            = headingTextInline?.Content.ToString() ?? "Unknown";

                toc.AppendLine($"<li><a href=\"#{id}\">{headingContent}</a></li>");
            }

            toc.AppendLine("</ul>");
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
    }
}
