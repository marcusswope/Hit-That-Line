using System.Collections.Generic;
using System.Security.Principal;
using FubuCore;
using HitThatLine.Infrastructure.Security;
using Newtonsoft.Json;

namespace HitThatLine.Domain.Accounts
{
    public class UserAccount
    {
        public const string LoginCookieName = "HTLLogin";
        public const string TimeZoneCookieName = "timeZoneOffset";
        public const string BasicUserRole = "user";

        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string EmailHash { get; set; }

        public string DocumentKey
        {
            get { return Key(Username); }
        }
        public static string Key(string username)
        {
            return "users/" + username;
        }

        [JsonIgnore]
        public virtual GenericPrincipal Principal
        {
            get
            {
                return _principal ?? (_principal = new HTLPrincipal(this));
            }
        }
        private GenericPrincipal _principal;

        [JsonIgnore]
        public string ProfilePictureUrl
        {
            get { return "http://www.gravatar.com/avatar/{0}?d=identicon&r=pg&s=60".ToFormat(EmailHash); }
        }

        public List<string> Roles { get; set; }

        public UserAccount()
        {
            Roles = new List<string>();
        }
    }
}