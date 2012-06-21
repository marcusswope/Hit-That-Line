using System.Security.Principal;
using System.Web;
using HitThatLine.Domain.Accounts;
using HitThatLine.Services;
using Moq;
using NUnit.Framework;
using HitThatLine.Tests.Utility;

namespace HitThatLine.Tests.Domain.Accounts
{
    [TestFixture]
    public class UserAccountTest
    {
        [TestFixture]
        public class Login
        {
            [Test]
            public void SetsCookie()
            {
                var userAccount = new UserAccount { Username = "34956" };
                var cookieStorage = new Mock<ICookieStorage>();
                var httpContext = new Mock<HttpContextBase>();

                userAccount.Login(cookieStorage.Object, httpContext.Object);

                cookieStorage.Verify(x => x.Set(UserAccount.LoginCookieName, userAccount.DocumentKey));
            }

            [Test]
            public void SetsCurrentUser()
            {
                var userAccount = new UserAccount { Username = "34956" };
                var cookieStorage = new Mock<ICookieStorage>();
                var httpContext = new Mock<HttpContextBase>();

                userAccount.Login(cookieStorage.Object, httpContext.Object);

                httpContext.VerifySet(x => x.User = It.Is<GenericPrincipal>(p => p.Identity.Name == userAccount.Username && p.IsInRole("user")));
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

        [TestFixture]
        public class ProfilePicture
        {
            [Test]
            public void ReturnsGravatarUrlWithEmailHash()
            {
                var account = new UserAccount { EmailHash = "someHash" };
                account.ProfilePictureUrl.ShouldEqual("http://www.gravatar.com/avatar/someHash?d=identicon&r=pg&s=70");
            }
        }

    }
}