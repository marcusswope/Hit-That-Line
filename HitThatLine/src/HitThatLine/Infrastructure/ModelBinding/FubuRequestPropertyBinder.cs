using System.Reflection;
using FubuCore.Binding;
using FubuCore;
using FubuMVC.Core.Runtime;

namespace HitThatLine.Infrastructure.ModelBinding
{
    public class FubuRequestPropertyBinder : IPropertyBinder
    {
        public bool Matches(PropertyInfo property)
        {
            return property.PropertyType.CanBeCastTo<IFubuRequest>();
        }

        public void Bind(PropertyInfo property, IBindingContext context)
        {
            var user = context.Service<IFubuRequest>();
            property.SetValue(context.Object, user, null);
        }
    }
}