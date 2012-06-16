using AutoMapper;
using FubuMVC.Core.Continuations;
using HitThatLine.Web.Endpoints.Account.Models;
using HitThatLine.Web.Endpoints.Home.Models;
using HitThatLine.Web.Services;

namespace HitThatLine.Web.Endpoints.Account
{
    public class RegisterEndpoint
    {
        private readonly IUserAccountService _service;
        private readonly IMappingEngine _mapper;

        public RegisterEndpoint(IUserAccountService service, IMappingEngine mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public RegisterViewModel Register(RegisterRequest request)
        {
            return _mapper.Map<RegisterRequest, RegisterViewModel>(request);
        }

        public FubuContinuation Register(RegisterCommand command)
        {
            _service.CreateNew(command);
            return FubuContinuation.RedirectTo<HomeInputModel>();
        } 
    }
}