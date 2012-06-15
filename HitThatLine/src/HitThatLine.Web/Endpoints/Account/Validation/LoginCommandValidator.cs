using HitThatLine.Core.Accounts;
using HitThatLine.Web.Endpoints.Account.Models;
using HitThatLine.Web.Infrastructure.Conventions.Validation;
using Raven.Client;
using FluentValidation;
using System.Linq;

namespace HitThatLine.Web.Endpoints.Account.Validation
{
    public class LoginCommandValidator : ConventionBasedValidator<LoginCommand>
    {
        private readonly IDocumentSession _session;
        public LoginCommandValidator(IDocumentSession session)
            : this()
        {
            _session = session;
        }

        public LoginCommandValidator()
        {
            RuleFor(x => x.Username)
                .Must(BeAValidUsername)
                .WithMessage("Username or password is invalid");

            RuleFor(x => x.Password)
                .Must(BeAValidPassword)
                .WithMessage("Username or password is invalid");
        }

        private bool BeAValidUsername(string username)
        {
            return _session.Query<UserAccount>().Any(x => x.Username == username);
        }

        private bool BeAValidPassword(LoginCommand command, string password)
        {
            if (!BeAValidUsername(command.Username)) return false;
            var userAccount = _session.Query<UserAccount>().First(x => x.Username == command.Username);
            return userAccount.Password == password;
        }
    }
}