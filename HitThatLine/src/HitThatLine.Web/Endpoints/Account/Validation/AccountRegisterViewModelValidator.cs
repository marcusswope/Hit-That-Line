using HitThatLine.Core.Accounts;
using HitThatLine.Web.Endpoints.Account.Models;
using HitThatLine.Web.Infrastructure.Conventions.Validation;
using FluentValidation;
using Raven.Client;
using System.Linq;

namespace HitThatLine.Web.Endpoints.Account.Validation
{
    public class AccountRegisterViewModelValidator : ConventionBasedValidator<RegisterViewModel>
    {
        private readonly IDocumentSession _session;

        public AccountRegisterViewModelValidator(IDocumentSession session)
        {
            _session = session;

            RuleFor(x => x.EmailAddress)
                .Must(NotBeDuplicateEmail)
                .WithMessage("already in use");

            RuleFor(x => x.Username)
                .Must(NotBeDuplicateUsername)
                .WithMessage("already in use");

            RuleFor(x => x.ConfirmPassword)
                .Must((model, confirmPassword) => model.Password == confirmPassword)
                .WithMessage("must match password");
        }

        private bool NotBeDuplicateEmail(string emailAddress)
        {
            return !_session.Query<UserAccount>().Any(x => x.EmailAddress == emailAddress);
        }

        private bool NotBeDuplicateUsername(string username)
        {
            return !_session.Query<UserAccount>().Any(x => x.Username == username);
        }
    }
}