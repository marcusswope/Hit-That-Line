using HitThatLine.Core.Accounts;
using HitThatLine.Web.Utility;
using Raven.Client;

namespace HitThatLine.Web.Services
{
    public interface IUserAccountService
    {
        UserAccount GetLoggedOnUser();
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
    }
}