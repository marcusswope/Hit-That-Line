using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using FubuCore;
using FubuMVC.Core.Security;
using HitThatLine.Infrastructure.Security;
using HitThatLine.Services;
using Newtonsoft.Json;
using HitThatLine.Utility;

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
            get { return BuildDocumentKey(Username); }
        }
        public static string BuildDocumentKey(string username)
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