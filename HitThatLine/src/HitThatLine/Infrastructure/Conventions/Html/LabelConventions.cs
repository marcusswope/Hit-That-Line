using FubuMVC.Core.UI;

namespace HitThatLine.Infrastructure.Conventions.Html
{
    public class LabelConventions : HtmlConventionRegistry
    {
        public LabelConventions()
        {
            Labels.Always
                .Modify((request, tag) => tag.Attr("for", request.Accessor.Name));
        }
    }
}