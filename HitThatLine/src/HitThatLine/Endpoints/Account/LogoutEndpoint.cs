using FubuMVC.Core.Continuations;
using HitThatLine.Endpoints.Account.Models;
using HitThatLine.Endpoints.Home.Models;

namespace HitThatLine.Endpoints.Account
{
    public class LogoutEndpoint
    {
        public FubuContinuation Logout(LogoutRequest request)
        {
            request.UserAccount.Logout(request.Cookies);
            return FubuContinuation.RedirectTo<HomeRequest>();
        } 
    }
}