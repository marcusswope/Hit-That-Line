using System.Security.Principal;
using System.Web;
using HitThatLine.Domain.Accounts;
using HitThatLine.Infrastructure.Behaviors;
using HitThatLine.Services;
using HitThatLine.Tests.Utility;
using Moq;
using NUnit.Framework;
using Raven.Client;

namespace HitThatLine.Tests.Infrastructure.Behaviors
{
    [TestFixture]
    public class UserAccountBehaviorTest
    {
        [Test]
        public void DoesNothingIfNotLoggedIn()
        {
            var behavior = TestableUserAccountBehavior.Build();
            behavior.Cookies.Setup(x => x.Contains(UserAccount.LoginCookieName)).Returns(false);

            behavior.Invoke();

            behavior.HttpContext.VerifySet(x => x.User = It.IsAny<IPrincipal>(), Times.Never());
            behavior.MockInsideBehavior.VerifyInvoked();
        }

        [Test]
        public void SetCurrentUserIfLoggedIn()
        {
            var behavior = TestableUserAccountBehavior.Build();
            behavior.Cookies.Setup(x => x.Contains(UserAccount.LoginCookieName)).Returns(true);
            behavior.Cookies.Setup(x => x.Get(UserAccount.LoginCookieName)).Returns(behavior.User.DocumentKey);
            behavior.Session.Setup(x => x.Load<UserAccount>(behavior.User.DocumentKey)).Returns(behavior.User);
            
            behavior.Invoke();

            behavior.HttpContext.VerifySet(x => x.User = behavior.User.Principal);
            behavior.MockInsideBehavior.VerifyInvoked();
        }

        [Test]
        public void LogsOutIfCantFindUser()
        {
            var behavior = TestableUserAccountBehavior.Build();
            behavior.Cookies.Setup(x => x.Contains(UserAccount.LoginCookieName)).Returns(true);
            behavior.Cookies.Setup(x => x.Get(UserAccount.LoginCookieName)).Returns(behavior.User.DocumentKey);
            behavior.Session.Setup(x => x.Load<UserAccount>(behavior.User.DocumentKey)).Returns(null as UserAccount);
            
            behavior.Invoke();

            behavior.Cookies.Verify(x => x.Remove(UserAccount.LoginCookieName));
            behavior.MockInsideBehavior.VerifyInvoked();
        }

        [Test]
        public void DoesNothingOnPartialInvoke()
        {
            var behavior = TestableUserAccountBehavior.Build();
            behavior.Cookies.Setup(x => x.Contains(UserAccount.LoginCookieName)).Returns(false);

            behavior.InvokePartial();

            behavior.HttpContext.VerifySet(x => x.User = It.IsAny<IPrincipal>(), Times.Never());
            behavior.MockInsideBehavior.VerifyPartialInvoked();
        }
    }

    public class TestableUserAccountBehavior : UserAccountBehavior
    {
        public Mock<ICookieStorage> Cookies { get; private set; }
        public Mock<IDocumentSession> Session { get; private set; }
        public Mock<HttpContextBase> HttpContext { get; private set; }
        public UserAccount User { get; private set; }
        public MockActionBehavior MockInsideBehavior { get { return InsideBehavior as MockActionBehavior; } }

        public TestableUserAccountBehavior(Mock<ICookieStorage> cookieStorage, Mock<IDocumentSession> session, Mock<HttpContextBase> httpContext)
            : base(cookieStorage.Object, session.Object, httpContext.Object)
        {
            Cookies = cookieStorage;
            Session = session;
            HttpContext = httpContext;
            User = new UserAccount { Username = "user" };
            InsideBehavior = new MockActionBehavior();
        }

        public static TestableUserAccountBehavior Build()
        {
            return new TestableUserAccountBehavior(new Mock<ICookieStorage>(), new Mock<IDocumentSession>(), new Mock<HttpContextBase>());
        }
    }
}