using System.Web;
using HitThatLine.Domain.Accounts;
using HitThatLine.Endpoints.Account.Models;
using HitThatLine.Services;
using Moq;
using NUnit.Framework;
using Raven.Client;

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
            }
        }
    }

    internal class TestableUserAccountService : UserAccountService
    {
        public Mock<ICookieStorage> CookieStorage { get; private set; }
        public Mock<HttpContextBase> HttpContext { get; private set; }
        public Mock<IDocumentSession> Session { get; private set; }

        public TestableUserAccountService(Mock<ICookieStorage> cookieStorage, Mock<IDocumentSession> session, Mock<HttpContextBase> httpContext)
            : base(cookieStorage.Object, session.Object)
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