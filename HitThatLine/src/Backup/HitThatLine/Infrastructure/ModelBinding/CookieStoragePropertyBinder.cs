using System.Reflection;
using FubuCore;
using FubuCore.Binding;
using HitThatLine.Services;

namespace HitThatLine.Infrastructure.ModelBinding
{
    public class CookieStoragePropertyBinder : IPropertyBinder
    {
        public bool Matches(PropertyInfo property)
        {
            return property.PropertyType.CanBeCastTo<ICookieStorage>();
        }

        public void Bind(PropertyInfo property, IBindingContext context)
        {
            var cookies = context.Service<ICookieStorage>();
            property.SetValue(context.Object, cookies, null);
        }
    }
}