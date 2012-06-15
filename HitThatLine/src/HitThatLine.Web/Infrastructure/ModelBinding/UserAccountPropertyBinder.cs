using System.Reflection;
using FubuCore.Binding;
using FubuCore;
using HitThatLine.Core.Accounts;
using HitThatLine.Web.Services;
using Raven.Client;
using System.Linq;

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

            if (user == null)
            {
                var username = context.Data.ValueAs<string>("Username");
                if (username != null)
                {
                    user = context.Service<IDocumentSession>().Query<UserAccount>().FirstOrDefault(x => x.Username == username);
                }
            }

            property.SetValue(context.Object, user, null);
        }
    }
}