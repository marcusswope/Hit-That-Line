using System.Reflection;
using System.Web.Security;
using FubuCore.Binding;
using System.Linq;

namespace HitThatLine.Web.Infrastructure.ModelBinding
{
    public class PasswordPropertyBinder : IPropertyBinder
    {
        public bool Matches(PropertyInfo property)
        {
            return property.Name.Contains("Password") && 
                   property.DeclaringType.GetProperties().Any(x => x.Name.Contains("Username"));
        }

        public void Bind(PropertyInfo property, IBindingContext context)
        {
            var usernameProperty = context.Object.GetType().GetProperties().First(x => x.Name.Contains("Username"));
            var username = context.Data.ValueAs<string>(property.Name);
            var password = context.Data.ValueAs<string>(usernameProperty.Name);
            if (username == null || password == null) return;

            var passwordToHash = username + password;
            var hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(passwordToHash, "SHA1");

            property.SetValue(context.Object, hashedPassword, null);
        }
    }
}