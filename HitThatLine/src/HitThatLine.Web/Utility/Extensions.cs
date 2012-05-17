using FubuMVC.Core.View;
using HtmlTags;

namespace HitThatLine.Web.Utility
{
    public static class Extensions
    {
        public static HtmlTag Clear(this IFubuPage page, int height = 0)
        {
            var tag = new DivTag().AddClass("clear");
            if (height != 0)
            {
                tag.Style("height", height + "px");
            }
            return tag;
        }
    }
}