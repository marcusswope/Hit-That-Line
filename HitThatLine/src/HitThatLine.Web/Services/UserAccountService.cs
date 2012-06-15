using HitThatLine.Core.Accounts;
using HitThatLine.Web.Endpoints.Account.Models;
using HitThatLine.Web.Utility;
using Raven.Client;

namespace HitThatLine.Web.Services
{
    public interface IUserAccountService
    {
        UserAccount GetLoggedOnUser();
        void Login(UserAccount userAccount);
        void CreateNew(RegisterCommand model);
        void Logout(UserAccount userAccount);
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

        public UserAccount GetLoggedOnUser()
        {
            var userId = _cookieStorage.Get(AppSettings.LoginCookieName);
            if (userId == null) return null;

            return _session.Load<UserAccount>(userId);
        }

        public void Login(UserAccount userAccount)
        {
            _cookieStorage.Set(AppSettings.LoginCookieName, userAccount.Id);
        }

        public void CreateNew(RegisterCommand model)
        {
            var account = new UserAccount
                              {
                                  EmailAddress = model.EmailAddress,
                                  Password = model.Password,
                                  Username = model.Username
                              };
            _session.Store(account);
            _cookieStorage.Set(AppSettings.LoginCookieName, account.Id);
        }

        public void Logout(UserAccount userAccount)
        {
            _cookieStorage.Remove(AppSettings.LoginCookieName);
        }
    }
}