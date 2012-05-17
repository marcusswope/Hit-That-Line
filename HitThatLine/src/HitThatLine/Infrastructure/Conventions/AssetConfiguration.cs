using System.Net;
using FubuMVC.Core.Registration.DSL;

namespace HitThatLine.Infrastructure.Conventions
{
    public static class AssetConfiguration
    {
        public static void Configure(this AssetsExpression assets)
        {
            assets.Alias("styles").Is("site.css");
            assets.Alias("markdownStyles").Is("markdown.css");
          
            assets.Alias("jquery").Is("jquery-1.7.2.min.js");

            assets.Alias("markdownConverter").Is("Markdown.Converter.js");
            assets.Alias("markdownEditor").Is("Markdown.Editor.js");
            assets.Alias("markdownSanitizer").Is("Markdown.Sanitizer.js");
            assets.OrderedSet("markdown").Is("markdownConverter,markdownEditor,markdownSanitizer");

            assets.CombineAllUniqueAssetRequests();
        }
    }
}