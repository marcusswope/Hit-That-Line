using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using FubuMVC.Core.Security;
using HitThatLine.Infrastructure.Security;
using HitThatLine.Services;
using Newtonsoft.Json;

namespace HitThatLine.Domain.Accounts
{
    public class UserAccount
    {
        public const string LoginCookieName = "HTLLogin";
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
            get { return string.Format("http://www.gravatar.com/avatar/{0}?d=identicon&r=pg&s=70", EmailHash); }
        }

        public List<string> Roles { get; set; }

        public UserAccount()
        {
            Roles = new List<string>();
        }
    }
}