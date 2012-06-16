using FubuMVC.Core.Continuations;
using HitThatLine.Web.Endpoints.Account.Models;
using HitThatLine.Web.Endpoints.Home.Models;
using HitThatLine.Web.Services;

namespace HitThatLine.Web.Endpoints.Account
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
            _service.Logout(request.UserAccount);
            return FubuContinuation.RedirectTo<HomeInputModel>();
        } 
    }
}