using FubuCore.Binding;
using HitThatLine.Core.Accounts;
using HitThatLine.Web.Infrastructure.ModelBinding;
using HitThatLine.Web.Services;
using Moq;
using NUnit.Framework;
using HitThatLine.Web.Tests.Utility;
using Raven.Client;

namespace HitThatLine.Web.Tests.Infrastructure.ModelBinding
{
    [TestFixture]
    public class UserAccountPropertyBinderTest
    {
        [Test]
        public void OnlyAppliesToType_UserAccount()
        {
            var binder = new UserAccountPropertyBinder();
            binder.Matches(typeof(UserAccountBindingModel).GetProperty("UserAccount")).ShouldBeTrue();
            binder.Matches(typeof(UserAccountBindingModel).GetProperty("NotAUserAccount")).ShouldBeFalse();
        }

        [Test]
        public void FirstAttemptsToSetUserFromUserAccountService()
        {
            var binder = new UserAccountPropertyBinder();
            var userAccount = new UserAccount();
            var model = new UserAccountBindingModel();
            var context = new Mock<IBindingContext>();
            var service = new Mock<IUserAccountService>();

            context.Setup(x => x.Service<IUserAccountService>()).Returns(service.Object);
            service.Setup(x => x.GetLoggedOnUser()).Returns(userAccount);
            context.SetupGet(x => x.Object).Returns(model);

            binder.Bind(typeof(UserAccountBindingModel).GetProperty("UserAccount"), context.Object);
            model.UserAccount.ShouldEqual(userAccount);
        }

        [TestFixture]
        public class IfUserIsNotLoggedId : RavenTestBase
        {
            [Test]
            public void AttemptsToSetUserFromUsernameProperty()
            {
                var binder = new UserAccountPropertyBinder();
                var model = new UserAccountBindingModel { Username = DefaultUser.Username };
                var context = new Mock<IBindingContext>();
                var service = new Mock<IUserAccountService>();
                var contextValues = new Mock<IContextValues>();

                context.Setup(x => x.Service<IUserAccountService>()).Returns(service.Object);
                service.Setup(x => x.GetLoggedOnUser()).Returns(null as UserAccount);
                context.SetupGet(x => x.Data).Returns(contextValues.Object);
                contextValues.Setup(x => x.ValueAs<string>("Username")).Returns(DefaultUser.Username);
                context.Setup(x => x.Service<IDocumentSession>()).Returns(Session);
                context.SetupGet(x => x.Object).Returns(model);

                binder.Bind(typeof(UserAccountBindingModel).GetProperty("UserAccount"), context.Object);
                model.UserAccount.ShouldEqual(DefaultUser);
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