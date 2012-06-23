using System.Security.Cryptography;
using System.Text;
using System.Web;
using HitThatLine.Domain.Accounts;
using HitThatLine.Endpoints.Account.Models;
using HitThatLine.Services;
using Moq;
using NUnit.Framework;
using Raven.Client;
using HitThatLine.Tests.Utility;

namespace HitThatLine.Tests.Services
{
    [TestFixture]
    public class UserAccountServiceTest
    {
        [TestFixture]
        public class CreateNew
        {
            [Test]
            public void CreatesStoresAndSetsTheCookie()
            {
                var service = TestableUserAccountService.Build();
                var command = new RegisterCommand { Username = "user", Password = "password", EmailAddress = "email@email.com", Cookies = service.CookieStorage.Object, HttpContext = service.HttpContext.Object };

                var userAccount = service.CreateNew(command);

                service.Session.Verify(x => x.Store(userAccount, userAccount.DocumentKey));
                service.CookieStorage.Verify(x => x.Set(UserAccount.LoginCookieName, userAccount.DocumentKey));
                userAccount.Username.ShouldEqual(command.Username);
                userAccount.Password.ShouldEqual(command.Password);
                userAccount.EmailAddress.ShouldEqual(command.EmailAddress);
            }

            [Test]
            public void CreatesEmailHash()
            {
                var service = TestableUserAccountService.Build();
                var command = new RegisterCommand { Username = "user", Password = "password", EmailAddress = "email@email.com", Cookies = service.CookieStorage.Object, HttpContext = service.HttpContext.Object };

                var userAccount = service.CreateNew(command);

                using (var md5 = MD5.Create())
                {
                    var data = md5.ComputeHash(Encoding.UTF8.GetBytes(command.EmailAddress));
                    var expectedHash = new StringBuilder();
                    for (var i = 0; i < data.Length; i++)
                    {
                        expectedHash.Append(data[i].ToString("x2"));
                    }
                    userAccount.EmailHash.ShouldEqual(expectedHash.ToString());
                }
            }
        }
    }

    internal class TestableUserAccountService : UserAccountService
    {
        public Mock<ICookieStorage> CookieStorage { get; private set; }
        public Mock<HttpContextBase> HttpContext { get; private set; }
        public Mock<IDocumentSession> Session { get; private set; }

        public TestableUserAccountService(Mock<ICookieStorage> cookieStorage, Mock<IDocumentSession> session, Mock<HttpContextBase> httpContext)
            : base(cookieStorage.Object, session.Object, httpContext.Object)
        {
            HttpContext = httpContext;
            CookieStorage = cookieStorage;
            Session = session;
        }

        public static TestableUserAccountService Build()
        {
            return new TestableUserAccountService(new Mock<ICookieStorage>(), new Mock<IDocumentSession>(), new Mock<HttpContextBase>());
        }
    }
}