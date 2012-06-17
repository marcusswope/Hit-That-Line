using HitThatLine.Domain.Accounts;
using HitThatLine.Endpoints.Account.Models;
using HitThatLine.Infrastructure.Conventions.Validation;
using Raven.Client;
using FluentValidation;
using System.Linq;

namespace HitThatLine.Endpoints.Account.Validation
{
    public class LoginCommandValidator : ConventionBasedValidator<LoginCommand>
    {
        private readonly IDocumentSession _session;
        
        public LoginCommandValidator(IDocumentSession session)
        {
            _session = session;

            RuleFor(x => x.Username)
                .Must(BeAValidUsername)
                .When(x => !string.IsNullOrEmpty(x.Username) && !string.IsNullOrEmpty(x.Password))
                .WithMessage("Username or password is invalid");

            RuleFor(x => x.Password)
                .Must(BeAValidPassword)
                .When(x => !string.IsNullOrEmpty(x.Username) && !string.IsNullOrEmpty(x.Password))
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