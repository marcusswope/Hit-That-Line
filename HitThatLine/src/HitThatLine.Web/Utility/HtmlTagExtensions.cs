using FubuMVC.Core.Assets.Files;
using FubuMVC.Core.UI;
using FubuMVC.Core.View;
using HtmlTags;

namespace HitThatLine.Web.Utility
{
    public static class HtmlTagExtensions
    {
        public static HtmlTag Grid(this HtmlTag tag, int size = 0)
        {
            if (size != 0)
            {
                return tag.AddClass("grid_" + size);
            }
            return tag.AddClass("grid");
        }

        public static HtmlTag Prefix(this HtmlTag tag, int size)
        {
            return tag.AddClass("prefix_" + size);
        }

        public static HtmlTag Suffix(this HtmlTag tag, int size)
        {
            return tag.AddClass("suffix_" + size);
        }

        public static HtmlTag Clear(this IFubuPage page, int height = 0)
        {
            var tag = new DivTag().AddClass("clear");
            if (height != 0)
            {
                tag.Style("height", height + "px");
            }
            return tag;
        }

        public static HtmlTag SvgImage(this IFubuPage page, string svgImage, string fallbackImage = null)
        {
            var staticFileUrl = page.Urls.UrlForAsset(AssetFolder.images, svgImage).Replace("_content", "content");
            var objectTag = new HtmlTag("object")
                                .Attr("data", staticFileUrl)
                                .Attr("type", "image/svg+xml");
            if (!string.IsNullOrEmpty(fallbackImage))
            {
                objectTag.Append(page.Image(fallbackImage));
            }
            return objectTag;
        }

        public static HtmlTag AutoFocus(this HtmlTag tag)
        {
            return tag.Attr("autofocus", "autofocus");
        }
    }
}