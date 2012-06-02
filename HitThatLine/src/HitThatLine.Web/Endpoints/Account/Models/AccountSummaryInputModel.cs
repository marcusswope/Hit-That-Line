using HitThatLine.Core.Accounts;

namespace HitThatLine.Web.Endpoints.Account.Models
{
    public class AccountSummaryInputModel
    {
        public bool LoggedIn { get { return User != null; } }
        public UserAccount User { get; set; }
    }
}