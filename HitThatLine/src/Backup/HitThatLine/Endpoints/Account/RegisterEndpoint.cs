using System.Linq;
using System.Threading;
using AutoMapper;
using FubuMVC.Core.Continuations;
using HitThatLine.Domain.Accounts;
using HitThatLine.Endpoints.Account.Models;
using HitThatLine.Endpoints.Home.Models;
using HitThatLine.Services;
using Raven.Client;

namespace HitThatLine.Endpoints.Account
{
    public class RegisterEndpoint
    {
        private readonly IUserAccountService _service;
        private readonly IMappingEngine _mapper;
        private readonly IDocumentSession _session;

        public RegisterEndpoint(IUserAccountService service, IMappingEngine mapper, IDocumentSession session)
        {
            _service = service;
            _mapper = mapper;
            _session = session;
        }

        public RegisterViewModel Register(RegisterRequest request)
        {
            return _mapper.Map<RegisterRequest, RegisterViewModel>(request);
        }

        public FubuContinuation Register(RegisterCommand command)
        {
            _service.CreateNew(command);
            return FubuContinuation.RedirectTo<HomeRequest>();
        }

        public DuplicateUsernameResponse ValidateUsername(DuplicateUsernameCommand command)
        {
            var exists = _session.Query<UserAccount>().Any(x => x.Username == command.Username);
            return new DuplicateUsernameResponse { IsValid = !exists };
        }

        public DuplicateEmailAddressResponse ValidateEmailAddress(DuplicateEmailAddressCommand command)
        {
            var exists = _session.Query<UserAccount>().Any(x => x.EmailAddress == command.EmailAddress);
            return new DuplicateEmailAddressResponse { IsValid = !exists };
        }
    }
}