using FubuCore;
using HitThatLine.Core.Accounts;

namespace HitThatLine.Web.Endpoints.Account.Models
{
    public class SummaryRequest
    {
        public bool LoggedIn { get { return User != null; } }
        public UserAccount User { get; set; }
    }

    public class SummaryViewModel
    {
        public string UserName { get; set; }
        public bool IsLoggedIn { get; set; }

        public SummaryViewModel(SummaryRequest request)
        {
            IsLoggedIn = request.LoggedIn;
            request.User.IfNotNull(x => UserName = x.Username);
        }
    }
}