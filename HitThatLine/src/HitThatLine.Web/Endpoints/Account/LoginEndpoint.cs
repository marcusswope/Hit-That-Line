using AutoMapper;
using FubuMVC.Core.Continuations;
using HitThatLine.Web.Endpoints.Account.Models;
using HitThatLine.Web.Endpoints.Home.Models;
using HitThatLine.Web.Services;

namespace HitThatLine.Web.Endpoints.Account
{
    public class LoginEndpoint
    {
        private readonly IUserAccountService _service;
        private readonly IMappingEngine _mapper;

        public LoginEndpoint(IUserAccountService service, IMappingEngine mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public LoginViewModel Login(LoginRequest request)
        {
            return _mapper.Map<LoginRequest, LoginViewModel>(request);
        }

        public FubuContinuation Login(LoginCommand command)
        {
            _service.Login(command.UserAccount);
            return FubuContinuation.RedirectTo<HomeInputModel>();
        } 
    }
}