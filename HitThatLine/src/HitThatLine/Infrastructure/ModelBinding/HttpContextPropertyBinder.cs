using System.Reflection;
using System.Web;
using FubuCore;
using FubuCore.Binding;

namespace HitThatLine.Infrastructure.ModelBinding
{
    public class HttpContextPropertyBinder : IPropertyBinder
    {
        public bool Matches(PropertyInfo property)
        {
            return property.PropertyType.CanBeCastTo<HttpContextBase>();
        }

        public void Bind(PropertyInfo property, IBindingContext context)
        {
            var httpContext = context.Service<HttpContextBase>();
            property.SetValue(context.Object, httpContext, null);
        } 
    }
}