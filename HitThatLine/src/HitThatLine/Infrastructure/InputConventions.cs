using FluentValidation.Results;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.UI;
using FubuCore;
using FubuCore.Reflection;
using System.Linq;
using HitThatLine.Infrastructure.Validation.Attributes;
using HtmlTags;

namespace HitThatLine.Infrastructure
{
    public class InputConventions : HtmlConventionRegistry
    {
        public InputConventions()
        {
            Editors.Always
                .Modify((request, tag) => tag.Attr("id", request.Accessor.Name));

            Editors
                .If(x => x.Accessor.FieldName.Contains("Password") && x.Accessor.PropertyType.IsString())
                .Attr("type", "password");

            Editors
                .If(x => x.Accessor.FieldName.Contains("Email") && x.Accessor.PropertyType.IsString())
                .Attr("type", "email");

            Editors
                .If(x => x.Accessor.FieldName.Contains("Body") && x.Accessor.PropertyType.IsString())
                .Modify(x => x.TagName("textarea"));

            Editors
                .If(x => x.Accessor.InnerProperty.HasAttribute<RequiredAttribute>())
                .Modify(x => x.AddClass("required"));

            Editors
                .If(x => x.Accessor.InnerProperty.HasAttribute<MinLengthAttribute>())
                .Modify((request, tag) =>
                {
                    var length = request.Accessor.InnerProperty.GetAttribute<MinLengthAttribute>().Length;
                    tag.Attr("minlength", length);
                });

            Editors
                .If(x => x.Accessor.InnerProperty.HasAttribute<MaxLengthAttribute>())
                .Modify((request, tag) =>
                {
                    var length = request.Accessor.InnerProperty.GetAttribute<MaxLengthAttribute>().Length;
                    tag.Attr("maxlength", length);
                });

            Editors.Always
                .Modify((request, tag) =>
                            {
                                var result = request.Get<IFubuRequest>().Get<ValidationResult>();
                                if (result == null || result.IsValid) return;
                                var error = result.Errors.FirstOrDefault(x => x.PropertyName == request.Accessor.InnerProperty.Name);
                                if (error == null) return;

                                var errorLabel = new HtmlTag("label");
                                errorLabel.Text(error.ErrorMessage);
                                errorLabel.AddClass("error");
                                errorLabel.Attr("for", request.Accessor.InnerProperty.Name);
                                errorLabel.Attr("generated", "true");
                                tag.Next = errorLabel;
                            });
                
        }
    }
}