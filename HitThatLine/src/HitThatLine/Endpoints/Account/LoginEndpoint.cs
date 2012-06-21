using AutoMapper;
using FubuMVC.Core.Continuations;
using HitThatLine.Domain.Accounts;
using HitThatLine.Endpoints.Account.Models;
using HitThatLine.Endpoints.Home.Models;
using Raven.Client;
using System.Linq;

namespace HitThatLine.Endpoints.Account
{
    public class LoginEndpoint
    {
        private readonly IMappingEngine _mapper;
        private readonly IDocumentSession _session;

        public LoginEndpoint(IMappingEngine mapper, IDocumentSession session)
        {
            _mapper = mapper;
            _session = session;
        }

        public LoginViewModel Login(LoginRequest request)
        {
            return _mapper.Map<LoginRequest, LoginViewModel>(request);
        }

        public FubuContinuation Login(LoginCommand command)
        {
            var account = _session.Query<UserAccount>().First(x => x.Username == command.Username);
            account.Login(command.Cookies, command.HttpContext);

            return FubuContinuation.RedirectTo<HomeRequest>();
        } 
    }
}