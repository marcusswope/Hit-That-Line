using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace HitThatLine.Services
{
    public interface ICookieStorage
    {
        bool Contains(string name);
        string Get(string name);
        void Set(string name, string value);
        void Remove(string name);
    }

    public class CookieStorage : ICookieStorage
    {
        private readonly HttpContextBase _httpContext;
        public CookieStorage(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        public bool Contains(string name)
        {
            return _httpContext.Request.Cookies[name] != null;
        }

        public string Get(string name)
        {
            var httpCookie = _httpContext.Request.Cookies[name];
            if (httpCookie == null) return null;

            var encryptedBytes = Convert.FromBase64String(httpCookie.Value);
            var decryptedBytes = ProtectedData.Unprotect(encryptedBytes, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(decryptedBytes);
        }

        public void Set(string name, string value)
        {
            var plainBytes = Encoding.UTF8.GetBytes(value);
            var encryptedBytes = ProtectedData.Protect(plainBytes, null, DataProtectionScope.CurrentUser);
            _httpContext.Response.Cookies.Add(new HttpCookie(name)
                                                         {
                                                             Value = Convert.ToBase64String(encryptedBytes),
                                                             Expires = DateTime.Now.AddYears(20)
                                                         });
        }

        public void Remove(string name)
        {
            _httpContext.Response.Cookies.Add(new HttpCookie(name)
            {
                Expires = DateTime.Now.AddYears(-1)
            });
        }
    }
}