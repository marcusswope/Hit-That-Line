using HitThatLine.Core.Accounts;

namespace HitThatLine.Web.Endpoints.Account.Models
{
    public class LogoutRequest
    {
        public UserAccount UserAccount { get; set; }
    }
}