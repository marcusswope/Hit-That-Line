using System.Security.Cryptography;
using System.Text;
using System.Web;
using HitThatLine.Domain.Accounts;
using HitThatLine.Endpoints.Account.Models;
using HitThatLine.Infrastructure.Security;
using HitThatLine.Utility;
using Raven.Client;

namespace HitThatLine.Services
{
    public interface IUserAccountService
    {
        UserAccount GetCurrent();
        UserAccount CreateNew(RegisterCommand command);
        void Login(UserAccount account);
        void Logout();
    }

    public class UserAccountService : IUserAccountService
    {
        private readonly ICookieStorage _cookieStorage;
        private readonly IDocumentSession _session;
        private readonly HttpContextBase _httpContext;

        public UserAccountService(ICookieStorage cookieStorage, IDocumentSession session, HttpContextBase httpContext)
        {
            _cookieStorage = cookieStorage;
            _session = session;
            _httpContext = httpContext;
        }

        public UserAccount GetCurrent()
        {
            var principal = _httpContext.User as HTLPrincipal;
            if (principal != null)
            {
                return principal.UserAccount;
            }
            if (_cookieStorage.Contains(UserAccount.LoginCookieName))
            {
                var userKey = _cookieStorage.Get(UserAccount.LoginCookieName);
                var user = _session.Load<UserAccount>(userKey);

                if (user != null)
                {
                    _httpContext.User = user.Principal;
                    return user;
                }
                _cookieStorage.Remove(UserAccount.LoginCookieName);
            }

            return null;
        }
        
        public UserAccount CreateNew(RegisterCommand command)
        {
            var account = new UserAccount
                              {
                                  EmailAddress = command.EmailAddress,
                                  EmailHash = createEmailhash(command.EmailAddress),
                                  Password = command.Password,
                                  Username = command.Username
                              };
            account.Roles.Add(UserAccount.BasicUserRole);
            _session.Store(account, UserAccount.Key(account.Username));
            Login(account);
            return account;
        }

        public void Login(UserAccount account)
        {
            _cookieStorage.Set(UserAccount.LoginCookieName, account.DocumentKey);
            _httpContext.User = account.Principal;
        }

        public void Logout()
        {
            _cookieStorage.Remove(UserAccount.LoginCookieName);
        }

        private static string createEmailhash(string email)
        {
            using (var md5 = MD5.Create())
            {
                var data = md5.ComputeHash(Encoding.UTF8.GetBytes(email));
                var expectedHash = new StringBuilder();
                for (var i = 0; i < data.Length; i++)
                {
                    expectedHash.Append(data[i].ToString("x2"));
                }
                return expectedHash.ToString();
            }
        }
    }
}