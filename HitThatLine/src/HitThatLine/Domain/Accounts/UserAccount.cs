using System.Security.Principal;
using System.Web;
using HitThatLine.Services;

namespace HitThatLine.Domain.Accounts
{
    public class UserAccount
    {
        public const string LoginCookieName = "HTLLogin";

        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }

        public string DocumentKey
        {
            get { return BuildDocumentKey(Username); }
        }
        public static string BuildDocumentKey(string username)
        {
            return "users/" + username;
        }

        public virtual void Login(ICookieStorage cookieStorage, HttpContextBase context)
        {
            cookieStorage.Set(LoginCookieName, DocumentKey);
            context.User = Principal;
        }

        private GenericPrincipal _principal;
        public virtual GenericPrincipal Principal
        {
            get
            {
                return _principal ?? (_principal = new GenericPrincipal(new GenericIdentity(Username), new[] { "user" }));
            }
        }

        public virtual void Logout(ICookieStorage cookieStorage)
        {
            cookieStorage.Remove(LoginCookieName);
        }
    }
}