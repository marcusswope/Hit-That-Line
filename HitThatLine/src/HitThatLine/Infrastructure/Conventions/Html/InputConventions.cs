using FubuMVC.Core.UI;
using FubuCore;

namespace HitThatLine.Infrastructure.Conventions.Html
{
    public class InputConventions : HtmlConventionRegistry
    {
        public InputConventions()
        {
            Editors
                .If(x => x.Accessor.FieldName.Contains("Password") && x.Accessor.PropertyType.IsString())
                .Attr("type", "password");

            Editors
                .If(x => x.Accessor.FieldName.Contains("Email") && x.Accessor.PropertyType.IsString())
                .Attr("type", "email");
        }
    }
}