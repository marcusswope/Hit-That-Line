using HitThatLine.Domain.Accounts;
using HitThatLine.Services;
using Moq;
using NUnit.Framework;

namespace HitThatLine.Tests.Domain.Accounts
{
    [TestFixture]
    public class UserAccountTest
    {
        [TestFixture]
        public class Login
        {
            [Test]
            public void SetCookie()
            {
                var userAccount = new UserAccount { Id = "34956" };
                var cookieStorage = new Mock<ICookieStorage>();

                userAccount.Login(cookieStorage.Object);

                cookieStorage.Verify(x => x.Set(UserAccount.LoginCookieName, userAccount.Id));
            }
        }

        [TestFixture]
        public class Logout
        {
            [Test]
            public void RemoveCookie()
            {
                var userAccount = new UserAccount();
                var cookieStorage = new Mock<ICookieStorage>();

                userAccount.Logout(cookieStorage.Object);

                cookieStorage.Verify(x => x.Remove(UserAccount.LoginCookieName));
            }
        }

    }
}