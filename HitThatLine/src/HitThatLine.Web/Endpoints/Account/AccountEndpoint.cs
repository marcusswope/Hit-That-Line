using HitThatLine.Web.Endpoints.Account.Models;

namespace HitThatLine.Web.Endpoints.Account
{
    public class AccountEndpoint
    {
        public AccountSummaryViewModel Summary(AccountSummaryInputModel input)
        {
            return new AccountSummaryViewModel
                       {
                           IsLoggedIn = input.LoggedIn,
                           UserName = input.User.Username
                       };
        }
    }
}