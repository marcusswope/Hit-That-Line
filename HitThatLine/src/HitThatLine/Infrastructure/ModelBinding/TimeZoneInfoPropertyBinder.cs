using System;
using System.Reflection;
using FubuCore;
using FubuCore.Binding;
using HitThatLine.Domain.Accounts;
using HitThatLine.Services;

namespace HitThatLine.Infrastructure.ModelBinding
{
    public class TimeZoneInfoPropertyBinder : IPropertyBinder
    {
        private readonly ICookieStorage _cookieStorage;
        public TimeZoneInfoPropertyBinder(ICookieStorage cookieStorage)
        {
            _cookieStorage = cookieStorage;
        }

        public bool Matches(PropertyInfo property)
        {
            return property.PropertyType.CanBeCastTo<TimeSpan>();
        }

        public void Bind(PropertyInfo property, IBindingContext context)
        {
            if (_cookieStorage.Contains(UserAccount.TimeZoneCookieName))
            {
                var offsetCookieValue = _cookieStorage.Get(UserAccount.TimeZoneCookieName, false);
                int offsetInMinutes;
                var parsed = int.TryParse(offsetCookieValue, out offsetInMinutes);
                if (parsed)
                {
                    var offset = new TimeSpan(0, offsetInMinutes, 0);
                    property.SetValue(context.Object, offset, null);
                }
            }
        }
    }
}