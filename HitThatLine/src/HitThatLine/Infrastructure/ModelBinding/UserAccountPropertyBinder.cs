using System.Reflection;
using FubuCore.Binding;
using FubuCore;
using HitThatLine.Domain.Accounts;
using HitThatLine.Services;

namespace HitThatLine.Infrastructure.ModelBinding
{
    public class UserAccountPropertyBinder : IPropertyBinder
    {
        public bool Matches(PropertyInfo property)
        {
            return property.PropertyType.CanBeCastTo<UserAccount>();
        }

        public void Bind(PropertyInfo property, IBindingContext context)
        {
            var account = context.Service<IUserAccountService>().GetCurrent();
            property.SetValue(context.Object, account, null);
        }
    }
}