using HitThatLine.Core.Accounts;

namespace HitThatLine.Web.Endpoints.Account.Models
{
    public class SummaryInputModel
    {
        public bool LoggedIn { get { return User != null; } }
        public UserAccount User { get; set; }
    }
}