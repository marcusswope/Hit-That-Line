using System.Reflection;
using System.Web.Security;
using FubuCore.Binding;
using System.Linq;

namespace HitThatLine.Infrastructure.ModelBinding
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
            var usernameProperty = context.Object.GetType().GetProperty("Username");
            var password = context.Data.ValueAs<string>(property.Name);
            var username = context.Data.ValueAs<string>(usernameProperty.Name);
            if (password == null || username == null) return;

            var passwordToHash = password + username;
            var hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(passwordToHash, "SHA1");

            property.SetValue(context.Object, hashedPassword, null);
        }
    }
}