using System.Security.Cryptography;
using System.Text;
using HitThatLine.Domain.Accounts;
using HitThatLine.Endpoints.Account.Models;
using HitThatLine.Utility;
using Raven.Client;

namespace HitThatLine.Services
{
    public interface IUserAccountService
    {
        UserAccount CreateNew(RegisterCommand command);
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
        
        public UserAccount CreateNew(RegisterCommand command)
        {
            var account = new UserAccount
                              {
                                  EmailAddress = command.EmailAddress,
                                  EmailHash = createEmailhash(command.EmailAddress),
                                  Password = command.Password,
                                  Username = command.Username
                              };

            _session.Store(account, UserAccount.BuildDocumentKey(account.Username));
            account.Login(_cookieStorage, command.HttpContext);
            return account;
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