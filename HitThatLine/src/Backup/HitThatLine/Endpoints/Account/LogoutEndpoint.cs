using FubuMVC.Core.Continuations;
using HitThatLine.Endpoints.Account.Models;
using HitThatLine.Endpoints.Home.Models;
using HitThatLine.Services;

namespace HitThatLine.Endpoints.Account
{
    public class LogoutEndpoint
    {
        private readonly IUserAccountService _service;

        public LogoutEndpoint(IUserAccountService service)
        {
            _service = service;
        }

        public FubuContinuation Logout(LogoutRequest request)
        {
            _service.Logout();
            return FubuContinuation.RedirectTo<HomeRequest>();
        } 
    }
}