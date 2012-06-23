using AutoMapper;
using FubuMVC.Core.Continuations;
using HitThatLine.Domain.Accounts;
using HitThatLine.Endpoints.Account.Models;
using HitThatLine.Endpoints.Home.Models;
using HitThatLine.Services;
using Raven.Client;
using System.Linq;

namespace HitThatLine.Endpoints.Account
{
    public class LoginEndpoint
    {
        private readonly IMappingEngine _mapper;
        private readonly IDocumentSession _session;
        private readonly IUserAccountService _service;

        public LoginEndpoint(IMappingEngine mapper, IDocumentSession session, IUserAccountService service)
        {
            _mapper = mapper;
            _session = session;
            _service = service;
        }

        public LoginViewModel Login(LoginRequest request)
        {
            return _mapper.Map<LoginRequest, LoginViewModel>(request);
        }

        public FubuContinuation Login(LoginCommand command)
        {
            var account = _session.Query<UserAccount>().First(x => x.Username == command.Username);
            _service.Login(account);

            return FubuContinuation.RedirectTo<HomeRequest>();
        } 
    }
}