using AutoMapper;
using FubuMVC.Core.Continuations;
using HitThatLine.Endpoints.Account.Models;
using HitThatLine.Endpoints.Home.Models;

namespace HitThatLine.Endpoints.Account
{
    public class LoginEndpoint
    {
        private readonly IMappingEngine _mapper;
        
        public LoginEndpoint(IMappingEngine mapper)
        {
            _mapper = mapper;
        }

        public LoginViewModel Login(LoginRequest request)
        {
            return _mapper.Map<LoginRequest, LoginViewModel>(request);
        }

        public FubuContinuation Login(LoginCommand command)
        {
            command.UserAccount.Login(command.Cookies, command.HttpContext);
            return FubuContinuation.RedirectTo<HomeRequest>();
        } 
    }
}