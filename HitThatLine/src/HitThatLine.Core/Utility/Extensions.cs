using MarkdownSharp;

namespace HitThatLine.Core.Utility
{
    public static class Extensions
    {
        private static readonly Markdown markdown;
        static Extensions()
        {
            markdown = new Markdown();
        }

        public static string MarkdownTransform(this string markdownText)
        {
            return markdown.Transform(markdownText);
        }
    }
}