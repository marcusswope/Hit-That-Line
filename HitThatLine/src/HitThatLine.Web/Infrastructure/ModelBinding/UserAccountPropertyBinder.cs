using System.Reflection;
using FubuCore.Binding;
using FubuCore;
using HitThatLine.Core.Accounts;
using HitThatLine.Web.Services;

namespace HitThatLine.Web.Infrastructure.ModelBinding
{
    public class UserAccountPropertyBinder : IPropertyBinder
    {
        public bool Matches(PropertyInfo property)
        {
            return property.PropertyType.CanBeCastTo<UserAccount>();
        }

        public void Bind(PropertyInfo property, IBindingContext context)
        {
            var user = context.Service<IUserAccountService>().GetLoggedOnUser();
            property.SetValue(context.Object, user, null);
        }
    }
}