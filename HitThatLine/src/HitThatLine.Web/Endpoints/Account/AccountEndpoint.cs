using FubuMVC.Core.Continuations;
using HitThatLine.Core.Accounts;
using HitThatLine.Web.Endpoints.Account.Models;
using FubuCore;
using HitThatLine.Web.Endpoints.Home.Models;
using HitThatLine.Web.Services;
using System.Linq;

namespace HitThatLine.Web.Endpoints.Account
{
    public class AccountEndpoint
    {
        private readonly IUserAccountService _service;

        public AccountEndpoint(IUserAccountService service)
        {
            _service = service;
        }

        public SummaryViewModel Summary(SummaryRequest request)
        {
            return new SummaryViewModel(request);
        }

        public RegisterViewModel Register(RegisterRequest request)
        {
            return new RegisterViewModel(request);
        }

        public FubuContinuation Register(RegisterCommand command)
        {
            _service.CreateNew(command);
            return FubuContinuation.RedirectTo<HomeInputModel>();
        }

        public FubuContinuation Logout(LogoutRequest request)
        {
            _service.Logout(request.UserAccount);
            return FubuContinuation.RedirectTo<HomeInputModel>();
        }

        public LoginViewModel Login(LoginRequest model)
        {
            return new LoginViewModel(model);
        }

        public FubuContinuation Login(LoginCommand command)
        {
            _service.Login(command.UserAccount);
            return FubuContinuation.RedirectTo<HomeInputModel>();
        }
    }
}