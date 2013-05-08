using System.Reflection;
using System.Web;
using FubuCore.Binding;

namespace HitThatLine.Infrastructure.ModelBinding
{
    public class IPAddressPropertyBinder : IPropertyBinder
    {
        public bool Matches(PropertyInfo property)
        {
            return property.PropertyType == typeof (string) && property.Name == "IPAddress";
        }

        public void Bind(PropertyInfo property, IBindingContext context)
        {
            var httpContext = context.Service<HttpContextBase>();
            var ipAddress = httpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrWhiteSpace(ipAddress)) ipAddress = httpContext.Request.ServerVariables["HTTP_X_FORWARDED"];
            if (string.IsNullOrWhiteSpace(ipAddress)) ipAddress = httpContext.Request.UserHostAddress;

            property.SetValue(context.Object, ipAddress, null);
        }
    }
}