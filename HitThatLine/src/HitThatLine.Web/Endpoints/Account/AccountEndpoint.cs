using FluentValidation.Results;
using FubuMVC.Core;
using FubuMVC.Core.Continuations;
using HitThatLine.Core.Accounts;
using HitThatLine.Web.Endpoints.Account.Models;
using FubuCore;
using HitThatLine.Web.Endpoints.Home.Models;
using HitThatLine.Web.Services;
using Raven.Client;
using System.Linq;

namespace HitThatLine.Web.Endpoints.Account
{
    public class AccountEndpoint
    {
        private readonly IUserAccountService _service;
        private readonly IDocumentSession _session;

        public AccountEndpoint(IUserAccountService service, IDocumentSession session)
        {
            _service = service;
            _session = session;
        }

        public SummaryViewModel Summary(SummaryInputModel input)
        {
            var viewModel = new SummaryViewModel { IsLoggedIn = input.LoggedIn };
            input.User.IfNotNull(x => viewModel.UserName = x.Username);

            return viewModel;
        }

        public RegisterViewModel Register(RegisterInputModel input)
        {
            return new RegisterViewModel();
        }

        public FubuContinuation Create(RegisterViewModel input)
        {
            _service.CreateNew(input);
            return FubuContinuation.RedirectTo<HomeInputModel>();
        }

        public FubuContinuation Logout(LogoutInputModel input)
        {
            _service.Logout(input.UserAccount);
            return FubuContinuation.RedirectTo<HomeInputModel>();
        }

        public LoginViewModel Login(LoginInputModel model)
        {
            return new LoginViewModel(model);
        }

        public FubuContinuation PerformLogin(LoginViewModel input)
        {
            var userAccount = _session.Query<UserAccount>().FirstOrDefault(x => x.Username == input.Username);
            if (userAccount != null && userAccount.Password == input.Password)
            {
                _service.Login(userAccount);
                return FubuContinuation.RedirectTo<HomeInputModel>();
            }

            return FubuContinuation.TransferTo(new LoginInputModel(input));
        }
    }
}