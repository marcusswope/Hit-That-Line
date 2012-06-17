using HitThatLine.Domain.Accounts;
using HitThatLine.Endpoints.Account.Models;
using HitThatLine.Utility;
using Raven.Client;

namespace HitThatLine.Services
{
    public interface IUserAccountService
    {
        UserAccount CreateNew(RegisterCommand model);
    }

    public class UserAccountService : IUserAccountService
    {
        private readonly ICookieStorage _cookieStorage;
        private readonly IDocumentSession _session;

        public UserAccountService(ICookieStorage cookieStorage, IDocumentSession session)
        {
            _cookieStorage = cookieStorage;
            _session = session;
        }
        
        public UserAccount CreateNew(RegisterCommand model)
        {
            var account = new UserAccount
                              {
                                  EmailAddress = model.EmailAddress,
                                  Password = model.Password,
                                  Username = model.Username
                              };
            _session.Store(account);
            _cookieStorage.Set(UserAccount.LoginCookieName, account.Id);
            return account;
        }
    }
}