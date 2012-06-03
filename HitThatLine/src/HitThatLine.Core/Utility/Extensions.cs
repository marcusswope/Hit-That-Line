using MarkdownSharp;
using System.Linq;

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

        public static bool IsUpperOrNumber(this char value)
        {
            int parser;
            var result = int.TryParse(value.ToString(), out parser);
            return result || value.ToString().ToUpper() == value.ToString();
        }

        public static bool IsUpper(this char value)
        {
            return value.ToString().ToUpper() == value.ToString();
        }

        public static bool IsLower(this char value)
        {
            return value.ToString().ToLower() == value.ToString();
        }

        public static bool IsNumber(this char value)
        {
            int parser;
            return int.TryParse(value.ToString(), out parser);
        }

        public static string ToPrettyString(this object originalString)
        {
            if (originalString == null || string.IsNullOrEmpty(originalString.ToString()))
            {
                return string.Empty;
            }

            if (originalString.ToString().ToUpper() == originalString.ToString())
            {
                return originalString.ToString();
            }

            var newString = string.Empty;
            for(var index = 0; index < originalString.ToString().Length; index++)
            {
                var previous = index > 0 ? originalString.ToString()[index - 1] : default(char);
                var current = originalString.ToString()[index];
                var next = index + 1 <= originalString.ToString().Length - 1 ? originalString.ToString()[index + 1] : default(char);

                if (previous == default(char)) newString = newString + current;
                else if (current.IsUpperOrNumber() && previous.IsLower()) newString = newString + " " + current;
                else if (current.IsUpper() && previous.IsUpperOrNumber() && next.IsLower()) newString = newString + " " + current;
                else if (!current.IsNumber() && previous.IsNumber()) newString = newString + " " + current;
                else newString = newString + current;
            }

            return newString;
        }
    }
}