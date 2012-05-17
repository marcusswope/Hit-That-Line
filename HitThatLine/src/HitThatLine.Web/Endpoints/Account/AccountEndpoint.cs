using HitThatLine.Core.Accounts;
using HitThatLine.Web.Endpoints.Account.Models;
using Raven.Client;
using System.Linq;

namespace HitThatLine.Web.Endpoints.Account
{
    public class AccountEndpoint
    {
        private readonly IDocumentSession _session;
        public AccountEndpoint(IDocumentSession session)
        {
            _session = session;
        }

        public CreateAccountViewModel New(NewAccountInputModel input)
        {
            return new CreateAccountViewModel();
        }

        public CreateAccountViewModel Create(CreateAccountCommand input)
        {
            var accountExists = _session.Query<UserAccount>().Any(x => x.EmailAddress == input.EmailAddress);
            if (!accountExists)
            {
                _session.Store(new UserAccount
                                   {
                                       EmailAddress = input.EmailAddress,
                                       Password = input.Password
                                   });
            }
            return new CreateAccountViewModel();
        }
    }
}