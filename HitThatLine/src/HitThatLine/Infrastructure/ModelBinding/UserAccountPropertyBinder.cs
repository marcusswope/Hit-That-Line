using System.Reflection;
using System.Web;
using FubuCore.Binding;
using FubuCore;
using HitThatLine.Domain.Accounts;
using HitThatLine.Infrastructure.Security;
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
            var principal = context.Service<HttpContextBase>().User as HTLPrincipal;
            
            if (principal != null)
            {
                property.SetValue(context.Object, principal.UserAccount, null);
            }
            else if (context.Service<ICookieStorage>().Contains(UserAccount.LoginCookieName))
            {
                var userKey = context.Service<ICookieStorage>().Get(UserAccount.LoginCookieName);
                var user = context.Service<IDocumentSession>().Load<UserAccount>(userKey);
                property.SetValue(context.Object, user, null);
            }
        }
    }
}