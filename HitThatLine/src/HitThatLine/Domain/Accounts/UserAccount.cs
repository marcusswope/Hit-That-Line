using HitThatLine.Services;

namespace HitThatLine.Domain.Accounts
{
    public class UserAccount
    {
        public const string LoginCookieName = "HTLLogin";

        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }

        public virtual void Login(ICookieStorage cookieStorage)
        {
            cookieStorage.Set(LoginCookieName, Id);
        }

        public virtual void Logout(ICookieStorage cookieStorage)
        {
            cookieStorage.Remove(LoginCookieName);
        }
    }
}