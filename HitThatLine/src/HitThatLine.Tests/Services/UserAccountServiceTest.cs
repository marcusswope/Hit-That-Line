using System.Security.Cryptography;
using System.Text;
using System.Web;
using HitThatLine.Domain.Accounts;
using HitThatLine.Endpoints.Account.Models;
using HitThatLine.Infrastructure.Security;
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
        public class GetCurrent
        {
            [Test]
            public void FirstTriesToRetrieveFromHttpContext()
            {
                var service = TestableUserAccountService.Build();
                var userAccount = new UserAccount { Username = "user" };
                var htlPrincipal = new HTLPrincipal(userAccount);

                service.HttpContext.SetupGet(x => x.User).Returns(htlPrincipal);

                service.GetCurrent().ShouldEqual(userAccount);
            }

            [Test]
            public void ThenTriesToGetFromCookieValue()
            {
                var service = TestableUserAccountService.Build();
                var account = new UserAccount { Username = "user" };

                service.Cookies.Setup(x => x.Contains(UserAccount.LoginCookieName)).Returns(true);
                service.Cookies.Setup(x => x.Get(UserAccount.LoginCookieName, true)).Returns(account.DocumentKey);
                service.Session.Setup(x => x.Load<UserAccount>(account.DocumentKey)).Returns(account);

                service.GetCurrent().ShouldEqual(account);
                service.HttpContext.VerifySet(x => x.User = account.Principal);
            }

            [Test]
            public void RemovesCookieIfNotFound()
            {
                var service = TestableUserAccountService.Build();
                var account = new UserAccount { Username = "user" };

                service.Cookies.Setup(x => x.Contains(UserAccount.LoginCookieName)).Returns(true);
                service.Cookies.Setup(x => x.Get(UserAccount.LoginCookieName, true)).Returns(account.DocumentKey);
                service.Session.Setup(x => x.Load<UserAccount>(account.DocumentKey)).Returns(null as UserAccount);

                service.GetCurrent().ShouldEqual(null);
                service.HttpContext.VerifySet(x => x.User = account.Principal, Times.Never());
                service.Cookies.Verify(x => x.Remove(UserAccount.LoginCookieName));
            }
        }


        [TestFixture]
        public class CreateNew
        {
            [Test]
            public void CreatesStoresAndSetsTheCookie()
            {
                var service = TestableUserAccountService.Build();
                var command = new RegisterCommand { Username = "user", Password = "password", EmailAddress = "email@email.com", Cookies = service.Cookies.Object, HttpContext = service.HttpContext.Object };

                var userAccount = service.CreateNew(command);

                service.Session.Verify(x => x.Store(userAccount, userAccount.DocumentKey));
                service.Cookies.Verify(x => x.Set(UserAccount.LoginCookieName, userAccount.DocumentKey));
                userAccount.Username.ShouldEqual(command.Username);
                userAccount.Password.ShouldEqual(command.Password);
                userAccount.EmailAddress.ShouldEqual(command.EmailAddress);
            }

            [Test]
            public void CreatesEmailHash()
            {
                var service = TestableUserAccountService.Build();
                var command = new RegisterCommand { Username = "user", Password = "password", EmailAddress = "email@email.com", Cookies = service.Cookies.Object, HttpContext = service.HttpContext.Object };

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

        [TestFixture]
        public class Login
        {
            [Test]
            public void SetsCookieAndPrincipal()
            {
                var service = TestableUserAccountService.Build();
                var account = new UserAccount { Username = "user" };

                service.Login(account);

                service.Cookies.Verify(x => x.Set(UserAccount.LoginCookieName, account.DocumentKey));
                service.HttpContext.VerifySet(x => x.User = account.Principal);
            }
        }

        [TestFixture]
        public class Logout
        {
            [Test]
            public void RemovesCookie()
            {
                var service = TestableUserAccountService.Build();

                service.Logout();

                service.Cookies.Verify(x => x.Remove(UserAccount.LoginCookieName));
            }
        }

    }

    internal class TestableUserAccountService : UserAccountService
    {
        public Mock<ICookieStorage> Cookies { get; private set; }
        public Mock<HttpContextBase> HttpContext { get; private set; }
        public Mock<IDocumentSession> Session { get; private set; }

        public TestableUserAccountService(Mock<ICookieStorage> cookieStorage, Mock<IDocumentSession> session, Mock<HttpContextBase> httpContext)
            : base(cookieStorage.Object, session.Object, httpContext.Object)
        {
            HttpContext = httpContext;
            Cookies = cookieStorage;
            Session = session;
        }

        public static TestableUserAccountService Build()
        {
            return new TestableUserAccountService(new Mock<ICookieStorage>(), new Mock<IDocumentSession>(), new Mock<HttpContextBase>());
        }
    }
}