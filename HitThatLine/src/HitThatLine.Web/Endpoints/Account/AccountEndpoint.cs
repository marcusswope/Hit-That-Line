using FubuMVC.Core.Continuations;
using HitThatLine.Web.Endpoints.Account.Models;
using FubuCore;
using HitThatLine.Web.Endpoints.Home.Models;

namespace HitThatLine.Web.Endpoints.Account
{
    public class AccountEndpoint
    {
        public AccountSummaryViewModel Summary(AccountSummaryInputModel input)
        {
            var viewModel = new AccountSummaryViewModel { IsLoggedIn = input.LoggedIn };
            input.User.IfNotNull(x => viewModel.UserName = x.Username);

            return viewModel;
        }

        public AccountRegisterViewModel Register(AccountRegisterInputModel input)
        {
            return new AccountRegisterViewModel();
        }

        public FubuContinuation Create(AccountRegisterViewModel input)
        {
            return FubuContinuation.RedirectTo<HomeInputModel>();
        }
    }
}