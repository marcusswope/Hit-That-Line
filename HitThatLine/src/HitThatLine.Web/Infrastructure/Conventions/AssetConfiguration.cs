using FubuMVC.Core.Registration.DSL;

namespace HitThatLine.Web.Infrastructure.Conventions
{
    public static class AssetConfiguration
    {
        public static void Configure(this AssetsExpression assets)
        {
            assets.Alias("styles").Is("site.css");
            assets.Alias("markdownStyles").Is("markdown.css");
            assets.Alias("reset").Is("reset.css");
            assets.Alias("text").Is("text.css");
          
            assets.Alias("jquery").Is("jquery-1.7.2.js");
            assets.Alias("master").Is("master.js");
            assets.Alias("adapt").Is("adapt.min.js");

            assets.Alias("markdownConverter").Is("Markdown.Converter.js");
            assets.Alias("markdownEditor").Is("Markdown.Editor.js");
            assets.Alias("markdownSanitizer").Is("Markdown.Sanitizer.js");
            assets.OrderedSet("markdown").Is("markdownConverter,markdownEditor,markdownSanitizer");

            assets.CombineAllUniqueAssetRequests();
        }
    }
}