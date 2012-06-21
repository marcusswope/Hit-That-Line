using System.Reflection;
using FubuCore.Binding;
using FubuCore;
using HitThatLine.Domain.Accounts;
using HitThatLine.Services;
using Raven.Client;

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
            if (context.Service<ICookieStorage>().Contains(UserAccount.LoginCookieName))
            {
                var userKey = context.Service<ICookieStorage>().Get(UserAccount.LoginCookieName);
                var user = context.Service<IDocumentSession>().Load<UserAccount>(userKey);
                property.SetValue(context.Object, user, null);
            }
        }
    }
}