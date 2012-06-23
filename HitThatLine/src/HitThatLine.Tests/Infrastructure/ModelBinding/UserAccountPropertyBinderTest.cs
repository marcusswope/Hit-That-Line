using System.Web;
using FubuCore.Binding;
using HitThatLine.Domain.Accounts;
using HitThatLine.Infrastructure.ModelBinding;
using HitThatLine.Infrastructure.Security;
using HitThatLine.Services;
using HitThatLine.Tests.Utility;
using Moq;
using NUnit.Framework;
using Raven.Client;

namespace HitThatLine.Tests.Infrastructure.ModelBinding
{
    [TestFixture]
    public class UserAccountPropertyBinderTest : RavenTestBase
    {
        [Test]
        public void OnlyAppliesToType_UserAccount()
        {
            var binder = new UserAccountPropertyBinder();
            binder.Matches(typeof(UserAccountBindingModel).GetProperty("UserAccount")).ShouldBeTrue();
            binder.Matches(typeof(UserAccountBindingModel).GetProperty("NotAUserAccount")).ShouldBeFalse();
        }

        [Test]
        public void FirstAttemptsToReadFromHttpContext()
        {
            var binder = TestableUserAccountPropertyBinder.Build(Session);

            binder.HttpContext.SetupGet(x => x.User).Returns(new HTLPrincipal(DefaultUser));

            binder.Bind(typeof(UserAccountBindingModel).GetProperty("UserAccount"), binder.Context.Object);

            binder.Model.UserAccount.ShouldEqual(DefaultUser);
        }

        [Test]
        public void ThenAttemptsToSetUserFromUserAccountService()
        {
            var binder = TestableUserAccountPropertyBinder.Build(Session);

            binder.Cookies.Setup(x => x.Contains(UserAccount.LoginCookieName)).Returns(true);
            binder.Cookies.Setup(x => x.Get(UserAccount.LoginCookieName)).Returns(DefaultUser.DocumentKey);

            binder.Bind(typeof(UserAccountBindingModel).GetProperty("UserAccount"), binder.Context.Object);
            
            binder.Model.UserAccount.ShouldEqual(DefaultUser);
        }

        private class TestableUserAccountPropertyBinder : UserAccountPropertyBinder
        {
            public Mock<IBindingContext> Context { get; private set; }
            public Mock<IContextValues> ContextValues { get; private set; }
            public Mock<ICookieStorage> Cookies { get; private set; }
            public Mock<HttpContextBase> HttpContext { get; private set; }
            public UserAccountBindingModel Model { get; private set; }
            
            public TestableUserAccountPropertyBinder(IDocumentSession session)
            {
                Context = new Mock<IBindingContext>();
                ContextValues = new Mock<IContextValues>();
                Cookies = new Mock<ICookieStorage>();
                Model = new UserAccountBindingModel();
                HttpContext = new Mock<HttpContextBase>();

                Context.Setup(x => x.Service<ICookieStorage>()).Returns(Cookies.Object);
                Context.Setup(x => x.Service<IDocumentSession>()).Returns(session);
                Context.Setup(x => x.Service<HttpContextBase>()).Returns(HttpContext.Object);
                Context.SetupGet(x => x.Object).Returns(Model);
                Context.SetupGet(x => x.Data).Returns(ContextValues.Object);
            }

            public static TestableUserAccountPropertyBinder Build(IDocumentSession session)
            {
                return new TestableUserAccountPropertyBinder(session);
            }
        }

        private class UserAccountBindingModel
        {
            public UserAccount UserAccount { get; set; }
            public string NotAUserAccount { get; set; }
            public string Username { get; set; }
        }
    }
}