using System.Security.Principal;
using System.Web;
using HitThatLine.Domain.Accounts;
using HitThatLine.Infrastructure.Security;
using HitThatLine.Services;
using HitThatLine.Tests.Utility;
using Moq;
using NUnit.Framework;
using Raven.Client;

namespace HitThatLine.Tests.Infrastructure.Security
{
    [TestFixture]
    public class UserAccountBehaviorTest
    {
        [Test]
        public void GetsFromServiceAndSetsPrincipal()
        {
            var behavior = TestableUserAccountBehavior.Build();
            var account = new UserAccount { Username = "user" };
            behavior.Service.Setup(x => x.GetCurrent()).Returns(account);

            behavior.Invoke();

            behavior.HttpContext.VerifySet(x => x.User = account.Principal);
            behavior.MockInsideBehavior.VerifyInvoked();
        }
    }

    public class TestableUserAccountBehavior : UserAccountBehavior
    {
        public Mock<IUserAccountService> Service { get; private set; }
        public Mock<HttpContextBase> HttpContext { get; private set; }
        public UserAccount User { get; private set; }
        public MockActionBehavior MockInsideBehavior { get { return InsideBehavior as MockActionBehavior; } }

        public TestableUserAccountBehavior(Mock<HttpContextBase> httpContext, Mock<IUserAccountService> service)
            : base(httpContext.Object, service.Object)
        {
            Service = service;
            HttpContext = httpContext;
            User = new UserAccount { Username = "user" };
            InsideBehavior = new MockActionBehavior();
        }

        public static TestableUserAccountBehavior Build()
        {
            return new TestableUserAccountBehavior(new Mock<HttpContextBase>(), new Mock<IUserAccountService>());
        }
    }
}