using HitThatLine.Core.Accounts;
using HitThatLine.Web.Endpoints.Account.Models;
using HitThatLine.Web.Services;
using HitThatLine.Web.Utility;
using Moq;
using NUnit.Framework;
using Raven.Client;
using HitThatLine.Web.Tests.Utility;

namespace HitThatLine.Web.Tests.Services
{
    [TestFixture]
    public class UserAccountServiceTest
    {
        [TestFixture]
        public class GetLoggedOnUser
        {
            [Test]
            public void LoadsUserIdFromCookies()
            {
                var service = TestableUserAccountService.Build();
                var userAccount = new UserAccount();

                service.CookieStorage.Setup(x => x.Get(AppSettings.LoginCookieName)).Returns("1234");
                service.Session.Setup(x => x.Load<UserAccount>("1234")).Returns(userAccount);

                service.GetLoggedOnUser().ShouldEqual(userAccount);
            }

            [Test]
            public void ReturnsNullIfNotLoggedIn()
            {
                var service = TestableUserAccountService.Build();

                service.CookieStorage.Setup(x => x.Get(AppSettings.LoginCookieName)).Returns(string.Empty);

                service.GetLoggedOnUser().ShouldBeNull();
            }
        }

        [TestFixture]
        public class Login
        {
            [Test]
            public void SetsTheCookie()
            {
                var service = TestableUserAccountService.Build();
                var userAccount = new UserAccount { Id = "1234" };

                service.Login(userAccount);

                service.CookieStorage.Verify(x => x.Set(AppSettings.LoginCookieName, userAccount.Id));
            }
        }

        [TestFixture]
        public class CreateNew
        {
            [Test]
            public void CreatesStoresAndSetsTheCookie()
            {
                var service = TestableUserAccountService.Build();
                var command = new RegisterCommand { Username = "user", Password = "password", EmailAddress = "email@email.com" };
                
                var userAccount = service.CreateNew(command);
                
                service.Session.Verify(x => x.Store(userAccount));
                service.CookieStorage.Verify(x => x.Set(AppSettings.LoginCookieName, userAccount.Id));
            }
        }

        [TestFixture]
        public class Logout
        {
            [Test]
            public void RemovesCookies()
            {
                var service = TestableUserAccountService.Build();

                service.Logout(new UserAccount());

                service.CookieStorage.Verify(x => x.Remove(AppSettings.LoginCookieName));
            }
        }
    }

    internal class TestableUserAccountService : UserAccountService
    {
        public Mock<ICookieStorage> CookieStorage { get; private set; }
        public Mock<IDocumentSession> Session { get; private set; }

        public TestableUserAccountService(Mock<ICookieStorage> cookieStorage, Mock<IDocumentSession> session)
            : base(cookieStorage.Object, session.Object)
        {
            CookieStorage = cookieStorage;
            Session = session;
        }

        public static TestableUserAccountService Build()
        {
            return new TestableUserAccountService(new Mock<ICookieStorage>(), new Mock<IDocumentSession>());
        }
    }
}